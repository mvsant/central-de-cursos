# Uso de Inteligência Artificial

## Ferramenta utilizada

Assistente de programação Copilot Luna, baseado no Copilot SDK e utilizado no Visual Studio Code. O Google Gemini também foi utilizado como apoio a pesquisas auxiliares via navegador.

## Objetivo do uso

A IA foi utilizada como apoio ao desenvolvimento e à documentação: esclarecer comportamentos de CSS e comandos de configuração, propor ajustes pontuais de interface, auxiliar na organização do modelo de categorias e gerar documentação técnica em português. As decisões finais e a integração ao projeto permaneceram sob revisão humana.

As pesquisas auxiliares via navegador com o Google Gemini serviram como apoio à consulta de referências. A adaptação de estilos e da marcação às necessidades e à estrutura do projeto foi feita manualmente.

## Prompts relevantes e resultado

Os exemplos abaixo resumem solicitações feitas durante o trabalho; não reproduzem credenciais, connection strings ou dados secretos.

| Solicitação resumida | O que foi aproveitado | O que foi descartado ou ajustado |
|---|---|---|
| Ajustar a animação da seta para deslizar horizontalmente sem mover o botão. | Animação limitada ao ícone em botões compatíveis. | A animação em setas de carrossel e o movimento do botão foram removidos após a revisão do comportamento. |
| Corrigir menu responsivo e finalizar a página de curso conforme o protótipo. | Ajustes de espaçamento e estados visuais, respeitando as páginas existentes. | O protótipo nem sempre carregou de forma confiável; comportamentos que não puderam ser observados foram tratados como referência, não como requisito confirmado. |
| Adicionar ícones e cores às categorias dos cursos e criar cursos fictícios. | Dados de exemplo foram usados para preencher o catálogo e alimentar a interface. | A primeira representação mantinha ícone e cor repetidos em cada curso; foi substituída por uma entidade compartilhada `CourseCategory`. |
| Separar categoria em um modelo próprio sem alterar o estilo. | Entidade, relacionamento, filtros, consultas e renderização foram atualizados preservando os valores visuais. | A primeira migração gerada pelo scaffolding removia os campos antigos sem copiá-los. Ela foi revisada para preencher a tabela de categorias e relacionar os cursos antes de remover as colunas antigas. |
| Explicar erro “Property value expected” em CSS. | A interpolação Razor foi mantida na view e o CSS passou a consumir valores por propriedades customizadas/estilos enviados pela view. | Razor direto em arquivo CSS (`@CategoryChipColor`) não é CSS válido e foi identificado como inadequado. |

## Tópico: relação entre `CourseViewModel` e `CourseCategory`

`CourseViewModel` continua representando os dados de cada curso e se relaciona com `CourseCategory` por `CategoryId` (chave estrangeira) e pela propriedade de navegação `Category`. Os dados compartilhados — nome, ícone e cor — ficam em `CourseCategory` para que vários cursos da mesma categoria usem uma única definição.

Na aplicação, as consultas MVC carregam a categoria relacionada antes de renderizar as páginas. A API projeta os dados relacionados de volta para campos de categoria planos, preservando a resposta existente para clientes. O `DbInitializer` mantém as categorias e relaciona cursos existentes ou novos a elas. A migração copia os valores antigos antes de excluir as colunas duplicadas.

## Como o conteúdo foi validado

- Build local do projeto com `dotnet build CeCursos\CeCursos.csproj --no-restore`; o build concluiu sem avisos ou erros após as alterações de modelo.
- `dotnet ef migrations has-pending-model-changes` confirmou que o snapshot correspondia ao modelo.
- Foi gerado e inspecionado um script SQL da migração de normalização de categorias, verificando que a cópia dos dados precedia a remoção das colunas antigas.
- O endpoint publicado `https://central-de-cursos.onrender.com/api/cursos` foi consultado e retornou a estrutura `total`/`resultados`, com 12 cursos e propriedades de categoria planas.
- Foram executadas verificações de diff e whitespace (`git diff --check`).

Essas verificações não substituem testes automatizados de integração ou uma validação manual completa em todos os navegadores e tamanhos de tela.

## Dificuldades e ajustes manuais

As sugestões de estilo não se encaixavam automaticamente em todas as regras já existentes no projeto. Foi necessário ajustar manualmente CSS e marcação HTML/Razor para preservar a estrutura, corrigir estados de interação e manter a apresentação consistente em diferentes larguras de tela. As alterações foram conferidas no código e validadas com build e verificações de diff; nem todas as combinações de navegador e dispositivo foram testadas.

## Exemplo de erro ou inadequação identificado

Uma migração inicialmente gerada propunha remover `Category`, `IconImage` e `CategoryChipColor` diretamente, enquanto criava a nova chave estrangeira. Aplicada daquela forma, perderia os valores existentes e deixaria cursos sem uma categoria válida.

## Correção realizada

A migração foi revisada para criar `Categories`, copiar os nomes, ícones e cores existentes, preencher `Courses.CategoryId`, tornar a referência obrigatória e só então remover as colunas antigas. O script SQL resultante foi revisado antes de a migração ser aplicada ao banco.

## Responsabilidade e segurança

As respostas da IA foram tratadas como sugestões e verificadas contra o código, as migrações e os resultados de build. Nenhuma senha ou connection string é documentada neste arquivo. Segredos de execução devem ser mantidos em variáveis de ambiente ou em um cofre de segredos, nunca no repositório.
