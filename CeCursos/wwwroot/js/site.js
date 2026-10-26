const navigationToggle = document.querySelector(".nav-toggle");
const navigation = document.querySelector("#primary-navigation");
const siteHeader = document.querySelector(".site-header");

navigationToggle?.addEventListener("click", () => {
    const isExpanded = navigationToggle.getAttribute("aria-expanded") === "true";
    navigationToggle.setAttribute("aria-expanded", String(!isExpanded));
    navigationToggle.setAttribute("aria-label", isExpanded ? "Abrir menu" : "Fechar menu");
    navigation?.classList.toggle("is-open", !isExpanded);
    siteHeader?.classList.toggle("is-menu-open", !isExpanded);
});

navigation?.querySelectorAll("a").forEach((link) => {
    link.addEventListener("click", () => {
        navigation.classList.remove("is-open");
        navigationToggle?.setAttribute("aria-expanded", "false");
        navigationToggle?.setAttribute("aria-label", "Abrir menu");
        siteHeader?.classList.remove("is-menu-open");
    });
});

const updateHeader = () => {
    siteHeader?.classList.toggle("is-scrolled", window.scrollY > 20);
};

updateHeader();
window.addEventListener("scroll", updateHeader, { passive: true });

document.querySelectorAll("[data-carousel]").forEach((carousel) => {
    const track = carousel.querySelector(".carousel-track");
    const previous = carousel.querySelector("[data-carousel-previous]");
    const next = carousel.querySelector("[data-carousel-next]");

    if (!(track instanceof HTMLElement) ||
        !(previous instanceof HTMLButtonElement) ||
        !(next instanceof HTMLButtonElement)) {
        return;
    }

    const updateControls = () => {
        const maxScroll = track.scrollWidth - track.clientWidth;
        previous.disabled = track.scrollLeft <= 1;
        next.disabled = track.scrollLeft >= maxScroll - 1;
    };

    const scrollBySlide = (direction) => {
        const firstSlide = track.querySelector(":scope > *");
        const slideWidth = firstSlide instanceof HTMLElement ? firstSlide.offsetWidth : track.clientWidth;
        const gap = Number.parseFloat(getComputedStyle(track).columnGap) || 0;

        track.scrollBy({
            left: direction * (slideWidth + gap),
            behavior: window.matchMedia("(prefers-reduced-motion: reduce)").matches ? "auto" : "smooth"
        });
    };

    previous.addEventListener("click", () => scrollBySlide(-1));
    next.addEventListener("click", () => scrollBySlide(1));
    track.addEventListener("scroll", updateControls, { passive: true });
    window.addEventListener("resize", updateControls);
    updateControls();
});

const revealSections = document.querySelectorAll(".reveal");
const impactCounters = [...document.querySelectorAll(".impact-stat strong[data-count]")]
    .filter((counter) => counter instanceof HTMLElement)
    .map((counter) => {
        const element = counter;
        const target = Number(element.dataset.count);
        const decimals = Number(element.dataset.countDecimals ?? 0);
        const suffix = element.dataset.countSuffix ?? "";

        element.textContent = `${new Intl.NumberFormat("pt-BR", {
            minimumFractionDigits: decimals,
            maximumFractionDigits: decimals
        }).format(0)}${suffix}`;

        return {
            element,
            target,
            decimals,
            suffix
        };
    });

const animateImpactCounters = (section) => {
    const counters = impactCounters.filter((counter) => section.contains(counter.element));
    const transitionDurations = getComputedStyle(section).transitionDuration
        .split(",")
        .map((duration) => {
            const value = Number.parseFloat(duration);
            return duration.trim().endsWith("ms") ? value : value * 1000;
        });
    const duration = Math.max(...transitionDurations);

    counters.forEach((counter) => {
        const formatter = new Intl.NumberFormat("pt-BR", {
            minimumFractionDigits: counter.decimals,
            maximumFractionDigits: counter.decimals
        });
        const render = (value) => {
            counter.element.textContent = `${formatter.format(value)}${counter.suffix}`;
        };

        if (duration <= 0 || window.matchMedia("(prefers-reduced-motion: reduce)").matches) {
            render(counter.target);
            return;
        }

        const startTime = performance.now();
        const update = (now) => {
            const progress = Math.min((now - startTime) / duration, 1);
            const easedProgress = 1 - (1 - progress) ** 3;
            render(counter.target * easedProgress);

            if (progress < 1) {
                window.requestAnimationFrame(update);
            } else {
                render(counter.target);
            }
        };

        window.requestAnimationFrame(update);
    });
};

if ("IntersectionObserver" in window) {
    document.documentElement.classList.add("reveal-ready");
    const revealObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach((entry) => {
            if (entry.isIntersecting) {
                entry.target.classList.add("is-visible");
                if (entry.target.classList.contains("impact-section")) {
                    animateImpactCounters(entry.target);
                }
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.12 });

    revealSections.forEach((section) => revealObserver.observe(section));
} else {
    revealSections.forEach((section) => section.classList.add("is-visible"));
    const impactSection = document.querySelector(".impact-section");
    if (impactSection instanceof HTMLElement) {
        animateImpactCounters(impactSection);
    }
}

const enrollmentButton = document.querySelector("[data-course-enroll]");
const stickyEnrollment = document.querySelector("[data-course-sticky-cta]");
if (enrollmentButton instanceof HTMLElement &&
    stickyEnrollment instanceof HTMLAnchorElement &&
    "IntersectionObserver" in window) {
    const enrollmentObserver = new IntersectionObserver(([entry]) => {
        stickyEnrollment.hidden = entry.isIntersecting;
        document.body.classList.toggle("has-sticky-course-cta", !entry.isIntersecting);
    });
    enrollmentObserver.observe(enrollmentButton);
}

const avaShell = document.querySelector("[data-ava-shell]");
const avaSidebar = document.querySelector("#ava-sidebar");
const avaCollapseButton = document.querySelector("[data-ava-collapse]");
const avaExpandButton = document.querySelector("[data-ava-expand]");
const avaBackdrop = document.querySelector("[data-ava-backdrop]");
const avaSearch = document.querySelector("[data-ava-search]");
const avaTopics = [...document.querySelectorAll("[data-ava-topic]")];
const avaTitle = document.querySelector("[data-ava-title-display]");
const avaContent = document.querySelector("[data-ava-content-display]");

if (avaShell instanceof HTMLElement &&
    avaSidebar instanceof HTMLElement &&
    avaCollapseButton instanceof HTMLButtonElement &&
    avaExpandButton instanceof HTMLButtonElement &&
    avaBackdrop instanceof HTMLButtonElement) {
    const isMobile = () => window.matchMedia("(max-width: 680px)").matches;
    const setSidebarCollapsed = (isCollapsed) => {
        avaShell.classList.toggle("is-sidebar-collapsed", isCollapsed);
        avaSidebar.inert = isCollapsed;
        avaSidebar.setAttribute("aria-hidden", String(isCollapsed));
        avaCollapseButton.setAttribute("aria-expanded", String(!isCollapsed));
        avaExpandButton.setAttribute("aria-expanded", String(!isCollapsed));
        avaExpandButton.hidden = !isCollapsed;
        avaBackdrop.hidden = isCollapsed || !isMobile();
    };

    avaCollapseButton.addEventListener("click", () => setSidebarCollapsed(true));
    avaExpandButton.addEventListener("click", () => setSidebarCollapsed(false));
    avaBackdrop.addEventListener("click", () => setSidebarCollapsed(true));
    window.addEventListener("resize", () => {
        const collapsed = avaShell.classList.contains("is-sidebar-collapsed");
        avaBackdrop.hidden = collapsed || !isMobile();
    });

    if (isMobile()) {
        setSidebarCollapsed(true);
    }

    avaTopics.forEach((topic) => {
        topic.addEventListener("click", () => {
            const selectedIndex = avaTopics.indexOf(topic);
            avaTopics.forEach((item, index) => {
                const state = index < selectedIndex
                    ? "completed"
                    : index === selectedIndex ? "active" : "not-started";
                item.dataset.avaState = state;
                item.setAttribute("aria-pressed", String(index === selectedIndex));
                item.classList.toggle("is-active", index === selectedIndex);
            });

            topic.closest("details")?.setAttribute("open", "");
            if (avaTitle instanceof HTMLElement) {
                avaTitle.textContent = topic.dataset.avaHeading ?? topic.dataset.avaTitle ?? "";
            }
            if (avaContent instanceof HTMLElement) {
                avaContent.textContent = topic.dataset.avaContent ?? "";
            }

            if (isMobile()) {
                setSidebarCollapsed(true);
            }
        });
    });

    avaSearch?.addEventListener("input", () => {
        if (!(avaSearch instanceof HTMLInputElement)) {
            return;
        }

        const query = avaSearch.value.trim().toLocaleLowerCase();
        avaSidebar.querySelectorAll("[data-ava-module]").forEach((module) => {
            const sectionMatches = module.querySelector("summary")?.textContent?.toLocaleLowerCase().includes(query) ?? false;
            const topics = [...module.querySelectorAll("[data-ava-topic]")];
            topics.forEach((topic) => {
                topic.hidden = query.length > 0 &&
                    !sectionMatches &&
                    !topic.textContent?.toLocaleLowerCase().includes(query);
            });

            const hasVisibleTopic = topics.some((topic) => !topic.hidden);
            module.hidden = query.length > 0 && !sectionMatches && !hasVisibleTopic;
        });

        const hasVisibleModule = [...avaSidebar.querySelectorAll("[data-ava-module]")]
            .some((module) => !module.hidden);
        const noResults = avaSidebar.querySelector("[data-ava-no-results]");
        if (noResults instanceof HTMLElement) {
            noResults.hidden = query.length === 0 || hasVisibleModule;
        }
    });
}
