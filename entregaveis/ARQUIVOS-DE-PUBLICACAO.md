# Relação de arquivos de publicação

O arquivo `central-de-cursos-fonte.zip`, nesta pasta, é um pacote de código-fonte para reconstrução e publicação. Ele não é o resultado compilado de `dotnet publish`.

## Arquivos centrais para publicação

- `Dockerfile` — restaura, compila e publica a aplicação em uma imagem .NET 10.
- `.dockerignore` — impede o envio de artefatos locais, configurações de desenvolvimento e arquivos de segredo ao contexto do Docker.
- `CeCursos/CeCursos.csproj` — dependências e configuração do projeto.
- `CeCursos/Program.cs` — inicialização da aplicação e configuração de serviços/rotas.
- `CeCursos/Controllers/` — páginas MVC e API HTTP.
- `CeCursos/Models/` — entidades e modelos de apresentação.
- `CeCursos/Data/` — contexto EF Core e inicialização dos dados fictícios.
- `CeCursos/Migrations/` — migrações versionadas do PostgreSQL.
- `CeCursos/Views/` e `CeCursos/wwwroot/` — interface Razor e recursos estáticos.
- `README.md` — configuração, execução, API, banco e fluxo de publicação.
- `USO_DE_IA.md` — registro do uso e validação de assistência por IA.

## Conteúdo e exclusões do ZIP

O ZIP inclui o código-fonte rastreado do projeto, Dockerfile, `.dockerignore`, migrações e documentação. Não inclui `.git`, diretórios `bin`/`obj`, connection strings, `.env` ou `appsettings.Development.json`.

Para gerar os binários publicados localmente, use `dotnet publish` conforme a configuração da máquina. Em produção, o Render realiza o build da imagem a partir do repositório.

