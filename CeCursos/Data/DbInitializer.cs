using CeCursos.Models;
using Microsoft.EntityFrameworkCore;

namespace cecursos.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(CecursosDbContext context)
    {
        var categories = new List<CourseCategory>
        {
            new()
            {
                Name = "Tecnologia",
                IconImage = "/assets/icon-tecnologia-36px.svg",
                ChipColor = "#e9e2f5"
            },
            new()
            {
                Name = "Gestão",
                IconImage = "/assets/icon-gestao-36.svg",
                ChipColor = "#f6e7ce"
            },
            new()
            {
                Name = "Design",
                IconImage = "/assets/icon-design-36px.svg",
                ChipColor = "#f5dfea"
            },
            new()
            {
                Name = "Comunicação",
                IconImage = "/assets/icon-comunicacao-36px.svg",
                ChipColor = "#e2edf5"
            },
            new()
            {
                Name = "Matemática",
                IconImage = "/assets/icon-matematica-20px.svg",
                ChipColor = "#ffc77f"
            },
            new()
            {
                Name = "Ciências da Natureza",
                IconImage = "/assets/icon-ciencias-natureza-20px.svg",
                ChipColor = "#e8d6f2"
            }
        };

        var existingCategories = await context.Categories.ToListAsync();
        foreach (var category in categories)
        {
            var existingCategory = existingCategories
                .FirstOrDefault(existing => existing.Name == category.Name);
            if (existingCategory is not null)
            {
                existingCategory.IconImage = category.IconImage;
                existingCategory.ChipColor = category.ChipColor;
                continue;
            }

            await context.Categories.AddAsync(category);
            existingCategories.Add(category);
        }

        await context.SaveChangesAsync();
        var categoryIds = existingCategories.ToDictionary(category => category.Name, category => category.Id);
        var today = DateTime.UtcNow.Date;
        var courses = new List<CourseViewModel>
        {
            new()
            {
                FullName = "Desenvolvimento de Aplicações Web",
                ShortName = "Web Essencial",
                Description = "Fundamentos para planejar e construir aplicações web do front-end ao back-end.",
                Modules = "1. HTML e acessibilidade\n2. CSS responsivo\n3. JavaScript e consumo de APIs\n4. Projeto integrador",
                LessonDescriptions = "HTML: criação de páginas semânticas.\nCSS: construção de layouts adaptáveis.\nJavaScript: validação de formulários e consumo de dados de uma API.",
                CategoryId = categoryIds["Tecnologia"],
                Modality = "EAD",
                Workload = 120,
                TargetAudience = "Pessoas iniciantes em programação e estudantes de tecnologia.",
                Status = "Inscrições abertas",
                PresentationImage = "/assets/tecnologia_img.jpg",
                AvailabilityDate = today
            },
            new()
            {
                FullName = "Fundamentos de Banco de Dados e SQL",
                ShortName = "SQL Prático",
                Description = "Aprenda a organizar informações e consultar bancos de dados relacionais.",
                Modules = "1. Modelagem de dados\n2. Consultas SELECT\n3. Junções e agregações\n4. Integridade e índices",
                LessonDescriptions = "Modelagem: elaboração de um diagrama relacional.\nConsultas: filtros, ordenação e paginação.\nJunções: combinação de dados de tabelas relacionadas.",
                CategoryId = categoryIds["Tecnologia"],
                Modality = "Presencial",
                Workload = 48,
                TargetAudience = "Estudantes, analistas iniciantes e profissionais que trabalham com dados.",
                Status = "Inscrições abertas",
                PresentationImage = "/assets/tecnologia_img.jpg",
                AvailabilityDate = today
            },
            new()
            {
                FullName = "Introdução à Ciência de Dados",
                ShortName = "Dados em Foco",
                Description = "Explore etapas de análise de dados, desde a preparação até a apresentação de resultados.",
                Modules = "1. Tipos e fontes de dados\n2. Limpeza de dados\n3. Análise exploratória\n4. Visualização de resultados",
                LessonDescriptions = "Preparação: identificação de valores ausentes e inconsistências.\nAnálise: cálculo de medidas descritivas.\nVisualização: escolha de gráficos para comunicar tendências.",
                CategoryId = categoryIds["Tecnologia"],
                Modality = "Híbrido",
                Workload = 72,
                TargetAudience = "Profissionais e estudantes interessados em análise de dados.",
                Status = "Em breve",
                PresentationImage = "/assets/tecnologia_img.jpg",
                AvailabilityDate = today.AddDays(20)
            },
            new()
            {
                FullName = "Gestão de Projetos com Métodos Ágeis",
                ShortName = "Projetos Ágeis",
                Description = "Conheça práticas para organizar equipes, acompanhar entregas e adaptar planos.",
                Modules = "1. Fundamentos de projetos\n2. Scrum e papéis\n3. Kanban e fluxo de trabalho\n4. Métricas e retrospectivas",
                LessonDescriptions = "Scrum: planejamento de uma sprint fictícia.\nKanban: criação de um quadro com limites de trabalho em andamento.\nRetrospectiva: identificação de melhorias para o próximo ciclo.",
                CategoryId = categoryIds["Gestão"],
                Modality = "EAD",
                Workload = 36,
                TargetAudience = "Líderes de equipe, estudantes e profissionais que apoiam projetos.",
                Status = "Inscrições abertas",
                PresentationImage = "/assets/gestao_img.jpg",
                AvailabilityDate = today
            },
            new()
            {
                FullName = "Comunicação e Liderança de Equipes",
                ShortName = "Liderança Colaborativa",
                Description = "Desenvolva comunicação clara, escuta ativa e práticas de colaboração em equipe.",
                Modules = "1. Comunicação interpessoal\n2. Escuta ativa\n3. Feedback construtivo\n4. Gestão de conflitos",
                LessonDescriptions = "Escuta ativa: exercício de reformulação de mensagens.\nFeedback: prática de conversas com foco em fatos e próximos passos.\nConflitos: simulação de negociação de prioridades.",
                CategoryId = categoryIds["Gestão"],
                Modality = "Presencial",
                Workload = 24,
                TargetAudience = "Pessoas que coordenam equipes ou desejam desenvolver habilidades de liderança.",
                Status = "Em breve",
                PresentationImage = "/assets/gestao_img.jpg",
                AvailabilityDate = today.AddDays(12)
            },
            new()
            {
                FullName = "Planejamento Financeiro para Pequenos Negócios",
                ShortName = "Finanças na Prática",
                Description = "Aprenda a estruturar controles financeiros básicos e acompanhar resultados.",
                Modules = "1. Fluxo de caixa\n2. Custos e despesas\n3. Formação de preço\n4. Indicadores financeiros",
                LessonDescriptions = "Fluxo de caixa: organização de entradas e saídas mensais.\nCustos: classificação de custos fixos e variáveis.\nPreço: exercício de cálculo de preço com custos fictícios.",
                CategoryId = categoryIds["Gestão"],
                Modality = "EAD",
                Workload = 40,
                TargetAudience = "Microempreendedores, estudantes e pessoas responsáveis por controles financeiros.",
                Status = "Inscrições encerradas",
                PresentationImage = "/assets/gestao_img.jpg",
                AvailabilityDate = null
            },
            new()
            {
                FullName = "Design de Interfaces para Produtos Digitais",
                ShortName = "Interface Digital",
                Description = "Planeje interfaces acessíveis e consistentes para produtos digitais.",
                Modules = "1. Princípios de design\n2. Arquitetura da informação\n3. Wireframes e protótipos\n4. Testes de usabilidade",
                LessonDescriptions = "Arquitetura: organização de conteúdo para uma tela inicial.\nProtótipos: criação de fluxo navegável de baixa fidelidade.\nUsabilidade: observação de tarefas realizadas por usuários fictícios.",
                CategoryId = categoryIds["Design"],
                Modality = "Híbrido",
                Workload = 64,
                TargetAudience = "Designers iniciantes, estudantes e profissionais de produto.",
                Status = "Inscrições abertas",
                PresentationImage = "/assets/design_img.jpg",
                AvailabilityDate = today
            },
            new()
            {
                FullName = "Ilustração Digital e Narrativa Visual",
                ShortName = "Ilustração Digital",
                Description = "Experimente técnicas digitais para desenvolver imagens e contar histórias visuais.",
                Modules = "1. Formas e composição\n2. Cor e luz\n3. Personagens e cenários\n4. Sequência visual",
                LessonDescriptions = "Composição: estudo de equilíbrio entre elementos.\nCor e luz: aplicação de paletas em uma cena.\nNarrativa: criação de uma sequência visual curta.",
                CategoryId = categoryIds["Design"],
                Modality = "EAD",
                Workload = 56,
                TargetAudience = "Pessoas interessadas em desenho, ilustração e criação visual.",
                Status = "Em breve",
                PresentationImage = "/assets/design_img.jpg",
                AvailabilityDate = today.AddDays(30)
            },
            new()
            {
                FullName = "Produção de Conteúdo para Mídias Digitais",
                ShortName = "Conteúdo Digital",
                Description = "Organize pautas e produza conteúdo adequado a diferentes canais digitais.",
                Modules = "1. Planejamento editorial\n2. Escrita para a web\n3. Formatos de conteúdo\n4. Calendário e métricas",
                LessonDescriptions = "Pauta: elaboração de um calendário editorial fictício.\nEscrita: adaptação de uma mensagem para diferentes canais.\nMétricas: leitura de indicadores básicos de alcance e interação.",
                CategoryId = categoryIds["Comunicação"],
                Modality = "Presencial",
                Workload = 32,
                TargetAudience = "Estudantes, empreendedores e profissionais de comunicação.",
                Status = "Inscrições encerradas",
                PresentationImage = "/assets/comunicacao_img.jpg",
                AvailabilityDate = null
            },
            new()
            {
                FullName = "Escrita Profissional e Revisão de Textos",
                ShortName = "Escrita Clara",
                Description = "Pratique a produção e revisão de textos claros para contextos profissionais.",
                Modules = "1. Objetivo e público do texto\n2. Estrutura e coesão\n3. Clareza e concisão\n4. Revisão e padronização",
                LessonDescriptions = "Estrutura: organização de um e-mail profissional.\nClareza: reescrita de instruções ambíguas.\nRevisão: aplicação de uma lista de verificação editorial.",
                CategoryId = categoryIds["Comunicação"],
                Modality = "Híbrido",
                Workload = 28,
                TargetAudience = "Estudantes e profissionais que produzem textos no trabalho.",
                Status = "Inscrições abertas",
                PresentationImage = "/assets/comunicacao_img.jpg",
                AvailabilityDate = today
            },
            new()
            {
                FullName = "Geometria e Modelagem no Design Têxtil",
                ShortName = "Design Têxtil",
                Description = "Aplique geometria, medidas e proporções para desenvolver padrões e modelos no design têxtil.",
                Modules = "1. Geometria aplicada aos padrões têxteis\n2. Medidas, escalas e proporções\n3. Modelagem e encaixe de peças\n4. Projeto de design têxtil sustentável",
                LessonDescriptions = "Padrões: construção de módulos geométricos para estampas.\nModelagem: uso de medidas, escalas e proporções na criação de moldes.\nEncaixe: planejamento do posicionamento dos moldes para reduzir sobras de tecido.\nProjeto: desenvolvimento de uma proposta têxtil considerando o aproveitamento de materiais.",
                CategoryId = categoryIds["Matemática"],
                Modality = "EAD",
                Workload = 40,
                TargetAudience = "Estudantes e profissionais de design, moda e produção têxtil.",
                Status = "Inscrições abertas",
                PresentationImage = "/assets/matematica_img.jpg",
                AvailabilityDate = today
            },
            new()
            {
                FullName = "Química e Sustentabilidade na Indústria Têxtil",
                ShortName = "Química Sustentável",
                Description = "Conheça os processos químicos da produção têxtil e explore práticas para reduzir impactos ambientais.",
                Modules = "1. Fibras e materiais têxteis\n2. Corantes e processos de tingimento\n3. Água, efluentes e impactos ambientais\n4. Práticas sustentáveis na produção têxtil",
                LessonDescriptions = "Fibras: comparação entre fibras naturais e sintéticas.\nTingimento: compreensão do papel de corantes e auxiliares químicos.\nEfluentes: identificação de impactos do uso da água nos processos têxteis.\nSustentabilidade: avaliação de alternativas para reduzir resíduos e consumo de recursos.",
                CategoryId = categoryIds["Ciências da Natureza"],
                Modality = "EAD",
                Workload = 48,
                TargetAudience = "Estudantes e profissionais interessados em química, sustentabilidade e indústria têxtil.",
                Status = "Inscrições abertas",
                PresentationImage = "/assets/ciencias_da_natureza_img.jpg",
                AvailabilityDate = today
            }
        };

        var existingCourses = await context.Courses.ToListAsync();

        foreach (var course in courses)
        {
            var existingCourse = existingCourses
                .FirstOrDefault(existing => existing.FullName == course.FullName);
            if (existingCourse is not null)
            {
                existingCourse.CategoryId = course.CategoryId;
                continue;
            }

            await context.Courses.AddAsync(course);
            existingCourses.Add(course);
        }

        await context.SaveChangesAsync();
    }
}
