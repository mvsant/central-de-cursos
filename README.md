# Central de Cursos

Aplicação web para consultar um catálogo fictício de cursos voltados à indústria. O projeto reúne a interface, a API e o acesso ao banco PostgreSQL em uma única aplicação ASP.NET Core MVC. Não depende de Moodle: a API de cursos é implementada e pode ser consultada diretamente.

## Acesso

- **Aplicação publicada:** [https://central-de-cursos.onrender.com/](https://central-de-cursos.onrender.com/)
- **API de cursos:** [https://central-de-cursos.onrender.com/api/cursos](https://central-de-cursos.onrender.com/api/cursos)
- **Repositório:** [https://github.com/mvsant/central-de-cursos](https://github.com/mvsant/central-de-cursos)
- **Referência visual:** protótipo SESI Indústria fornecido no Figma.

O endereço raiz é a aplicação web. A API está no caminho `/api/cursos`.

## Funcionalidades

- Catálogo responsivo, com páginas inicial, de cursos e de detalhes.
- Pesquisa pelo nome completo ou breve do curso.
- Filtros por categoria, modalidade e status, que podem ser combinados com a pesquisa.
- Exibição da quantidade de resultados e estado vazio quando nenhum curso corresponde aos filtros.
- Página individual com descrição, módulos, informações e conteúdo complementar.
- API HTTP para listar, pesquisar e filtrar cursos e consultar um curso pelo identificador.
- Categorias com nome, ícone e cor associados em uma tabela própria.
- Dados de exemplo fictícios, criados ou reconciliados pelo `DbInitializer`.

O catálogo é renderizado no servidor por MVC. Por isso, a interface não usa uma etapa assíncrona de carregamento nem exibe um indicador de carregamento entre filtros: cada consulta navega para uma resposta renderizada pelo servidor. Erros de aplicação são encaminhados pelo tratamento padrão do ASP.NET Core; a API responde com códigos HTTP apropriados, incluindo `404` para um curso inexistente.

## Tecnologias

- C# e ASP.NET Core MVC em .NET 10.
- Entity Framework Core 10 e provedor PostgreSQL do Npgsql.
- PostgreSQL hospedado no Neon.
- Razor, HTML, CSS e JavaScript para a interface.
- Docker para publicação no Render.

## Apoio no desenvolvimento

O desenvolvimento contou com apoio desta IA, identificada na atividade como Copilot Luna, e do Google Gemini para pesquisas auxiliares via navegador. As sugestões foram revisadas; a adaptação dos estilos ao projeto e às telas foi executada manualmente.

## Arquitetura

```text
Navegador
  ├── páginas MVC (Razor) ──┐
  └── API HTTP /api/cursos ─┤
                            v
                 ASP.NET Core + EF Core
                            |
                            v
                    PostgreSQL (Neon)
```

O site e a API fazem parte da mesma aplicação e compartilham o contexto do Entity Framework. As páginas MVC consultam o banco diretamente pelo contexto; a API oferece uma interface HTTP independente para clientes externos. A API devolve propriedades de categoria planas — `category`, `iconImage` e `categoryChipColor` — para manter o formato consumível pelos clientes.

## Estrutura do projeto

```text
.
├── Dockerfile
├── .dockerignore
├── README.md
├── USO_DE_IA.md
├── entregaveis/
│   ├── ARQUIVOS-DE-PUBLICACAO.md
│   └── central-de-cursos-fonte.zip
└── CeCursos/
    ├── Controllers/       # Endpoints MVC e API
    ├── Data/              # DbContext e DbInitializer
    ├── Migrations/        # Histórico e scripts de migração EF Core
    ├── Models/            # Entidades e view models
    ├── Views/             # Páginas Razor
    ├── wwwroot/           # CSS, JavaScript, imagens, ícones e fontes
    ├── Program.cs
    └── CeCursos.csproj
```

### Modelo de dados

- **CourseViewModel (tabela `Courses`):** nome completo e breve, descrição, módulos, descrições das aulas, categoria relacionada, modalidade, carga horária, público-alvo, status, imagem de apresentação e data de disponibilização.
- **CourseCategory (tabela `Categories`):** nome único da categoria, caminho do ícone e cor usada na interface.
- **Relacionamento:** cada curso possui uma categoria (`CategoryId`); uma categoria pode ser usada por vários cursos.

#### Relação entre `CourseViewModel` e `CourseCategory`

`CourseViewModel` é a entidade persistida de curso, apesar do sufixo `ViewModel` no nome. Ela contém os dados próprios de cada curso e mantém `CategoryId` como chave estrangeira e `Category` como propriedade de navegação. `CourseCategory` centraliza os dados compartilhados da categoria: `Name`, `IconImage` e `ChipColor`.

Essa relação evita repetir o mesmo nome, ícone e cor em todos os cursos de uma categoria. As páginas carregam a categoria relacionada para mostrar os mesmos dados que antes apareciam na interface. O endpoint da API continua expondo esses valores como propriedades planas (`category`, `iconImage` e `categoryChipColor`) para manter o formato de resposta consumido por clientes.

As migrações versionadas em `CeCursos/Migrations/` criam e atualizam o esquema. O `DbInitializer` cadastra/reconcilia as categorias e os cursos fictícios quando a aplicação inicia, mas **não aplica migrações automaticamente**.

## Executar localmente

Pré-requisitos:

- .NET SDK 10.
- Acesso a uma instância PostgreSQL compatível (local ou Neon).
- Ferramenta `dotnet-ef` 10.0.4 para executar comandos de migração, caso ainda não esteja instalada.

Na raiz do repositório:

```powershell
dotnet restore .\CeCursos\CeCursos.csproj
dotnet tool install --global dotnet-ef --version 10.0.4
dotnet user-secrets init --project .\CeCursos\CeCursos.csproj
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<SUA_CONNECTION_STRING_POSTGRES>" --project .\CeCursos\CeCursos.csproj
```

Se `dotnet-ef` já estiver instalado, não é necessário instalá-lo novamente. A connection string é um segredo: substitua o marcador localmente e nunca a inclua no código, no ZIP ou em commits.

Antes de iniciar a aplicação, aplique as migrações ao banco escolhido:

```powershell
dotnet ef database update --project .\CeCursos\CeCursos.csproj --startup-project .\CeCursos\CeCursos.csproj
```

Inicie a aplicação:

```powershell
dotnet watch --project .\CeCursos\CeCursos.csproj
```

O endereço local será informado no terminal. Para desenvolvimento, o ASP.NET Core usa `appsettings.Development.json` para configurações não secretas e User Secrets para a connection string.

## Configuração do banco

O banco de produção é PostgreSQL no [Neon](https://neon.tech/). Para executar localmente, use uma connection string PostgreSQL válida. Para aplicar as migrações de produção, execute `dotnet ef database update` apontando explicitamente para o banco de destino.

No Render, configure a variável de ambiente:

```text
ConnectionStrings__DefaultConnection=<connection string PostgreSQL do Neon>
```

O valor deve ser inserido diretamente no painel de variáveis do Render; não o registre neste arquivo ou no Git. O uso de planos gratuitos depende das condições, limites e disponibilidade vigentes dos provedores Neon e Render, que podem mudar.

## Endpoints

Base publicada: `https://central-de-cursos.onrender.com`

| Método | Caminho | Descrição |
|---|---|---|
| `GET` | `/api/cursos` | Lista cursos; retorna `total` e `resultados`. |
| `GET` | `/api/cursos?busca=web` | Pesquisa por nome completo ou breve. |
| `GET` | `/api/cursos?categoria=Tecnologia` | Filtra por categoria. |
| `GET` | `/api/cursos?modalidade=EAD` | Filtra por modalidade. |
| `GET` | `/api/cursos?status=Inscrições%20abertas` | Filtra por status. |
| `GET` | `/api/cursos?busca=web&categoria=Tecnologia&modalidade=EAD&status=Inscrições%20abertas` | Combina parâmetros de pesquisa e filtros. |
| `GET` | `/api/cursos/{id}` | Consulta um curso pelo identificador; retorna `404` se não existir. |
| `GET` | `/cursos` | Abre o catálogo web com pesquisa e filtros por formulário. |
| `GET` | `/cursos/{id}` | Abre os detalhes de um curso. |

Os nomes dos parâmetros da API são `busca`, `categoria`, `modalidade` e `status`. As categorias podem conter acentos e espaços; clientes HTTP devem codificar os valores da query string conforme necessário.

## Responsividade e acessibilidade

- Layouts e controles se adaptam a larguras diferentes por meio de media queries em `CeCursos/wwwroot/css/site.css`.
- A navegação, os carrosséis e os controles usam HTML semântico, nomes acessíveis, rótulos, estados e atributos `aria-*` quando apropriado.
- Imagens decorativas usam texto alternativo vazio; imagens informativas têm descrição.
- A navegação por teclado e o foco visível são considerados nos controles interativos.
- A preferência do sistema por movimento reduzido é respeitada nas animações.
- A categorização por cor é mantida como informação visual complementar ao nome da categoria; o nome textual permanece visível.

## Publicação

A aplicação é publicada no Render a partir do repositório GitHub usando o `Dockerfile` da raiz. O contêiner usa as imagens oficiais de SDK e ASP.NET Runtime .NET 10; a porta é configurada pelo Render através de `PORT`.

Fluxo de publicação:

1. Confirmar que as migrações necessárias estão aplicadas ao banco Neon de destino.
2. Enviar as alterações para o branch conectado ao serviço Render.
3. Configurar ou confirmar `ConnectionStrings__DefaultConnection` no painel do Render.
4. Aguardar o build e o deploy e conferir a página e `/api/cursos`.

O `DbInitializer` executa na inicialização e precisa encontrar o esquema atualizado. Não configure a aplicação para aplicar migrações automaticamente na inicialização.

O pacote `entregaveis/central-de-cursos-fonte.zip` contém o código-fonte e os arquivos necessários para reconstruir a aplicação com Docker. **Não é um binário compilado de `dotnet publish`**; o Render produz a publicação durante o build.

## Limitações conhecidas e diferenças em relação ao protótipo

- O Moodle não é instalado nem integrado; a API própria de cursos está implementada e funcional.
- O ambiente AVA é uma demonstração de navegação e conteúdo, sem autenticação, progresso persistido ou integração com uma plataforma educacional.
- A área de vídeo usa uma imagem de pré-visualização e indicação de disponibilidade; não há reprodução de vídeo integrada.
- O catálogo é renderizado no servidor; não há skeleton/indicador de carregamento assíncrono. A página oferece estado vazio quando a busca não encontra resultados.
- O atalho para limpar filtros aparece no estado sem resultados; ainda não há um controle dedicado de limpeza visível em todos os resultados filtrados.
- O conteúdo e os cursos são fictícios.
- A interface segue o protótipo como referência, mas pode apresentar diferenças de conteúdo e composição. A cor por categoria é aplicada a componentes relacionados para reforçar a identificação visual; os nomes também são exibidos para não depender apenas da cor.
- O uso de `aria-*`, estados acessíveis e foco visível complementa a referência visual para apoiar navegação assistiva e por teclado.
- Hospedagem gratuita e disponibilidade dependem das políticas e limites atuais dos provedores externos.

## Créditos visuais

Agradecimentos ao [Unsplash](https://unsplash.com/) pelas referências e imagens, ao [SVG Repo](https://www.svgrepo.com/) pelos recursos vetoriais e ao [Free SVG Editor](https://freesvgeditor.com/) pelas ferramentas de edição de SVG. Os arquivos usados localmente estão em `CeCursos/wwwroot/assets/`; consulte os créditos e licenças de cada recurso antes de reutilizá-lo fora deste projeto.
