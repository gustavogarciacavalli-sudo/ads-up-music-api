# Roteiro para o Vídeo de Apresentação (3 a 5 minutos)

### 1. Apresentação inicial
“Olá, este é o BeatFlow, nosso sistema de gestão musical desenvolvido para a disciplina de Tópicos Especiais em Sistemas. Nosso grupo é formado por Gustavo, Pedro, e Lucas.”

### 2. Mostrar o GitHub
- **Ação:** Abra o repositório no navegador.
- **Fala:** “Aqui está nosso repositório no GitHub. Trabalhamos com uma arquitetura separada de backend e frontend. (Mostre rapidamente os commits para evidenciar o trabalho da equipe).”

### 3. Mostrar backend
- **Ação:** Abra o Visual Studio ou VS Code mostrando a estrutura (Models, Controllers, DbContext).
- **Fala:** “No backend usamos C# com SDK .NET 8, criando uma API REST. Usamos Entity Framework com SQLite para persistência.”
- **Ação:** Mostre a tela do Swagger rodando no navegador (http://localhost:5270/swagger).
- **Fala:** “Aqui está o nosso Swagger, onde documentamos todos os nossos endpoints de Gêneros, Artistas, Músicas e Playlists.”

### 4. Mostrar frontend
- **Ação:** Abra a tela inicial (Dashboard) do frontend React (http://localhost:5173).
- **Fala:** “Para o frontend, desenvolvemos essa interface moderna em React usando TypeScript e CSS Vanilla.”
- **Ação:** Navegue até a tela de Gêneros e cadastre um novo.
- **Ação:** Vá para Artistas e cadastre um artista vinculado a esse gênero.
- **Ação:** Vá para Músicas e cadastre uma música vinculada ao artista. Mostre a validação (tente salvar sem BPM, por exemplo).
- **Ação:** Vá para Playlists, crie uma playlist.
- **Ação:** Entre nos detalhes dessa playlist recém-criada, adicione a música que você acabou de criar.
- **Fala:** “Na playlist podemos ver o resumo dinâmico sendo calculado: quantidade de músicas, BPM médio, duração total, e quais artistas e gêneros estão presentes.”
- **Ação:** Remova a música da playlist para mostrar a funcionalidade funcionando, ou edite/exclua um registro de outra tela para provar que o CRUD está completo.

### 5. Explicar requisitos atendidos
- **Fala:** “Para concluir, nosso projeto atende a todos os requisitos: 
  - Backend em C# .NET 8 com Entity Framework e SQLite.
  - Nossa API REST comunica em JSON.
  - Frontend React TypeScript que consome nossa API de forma local.
  - Temos as 4 entidades propostas (Gênero, Artista, Música e Playlist) com relacionamentos (1:N) e a lógica de negócio real na Playlist sem usar tabelas N:N intermediárias pesadas.
  - Validações de campos obrigatórios.
  - Atualizamos nosso README indicando como a IA foi usada para estruturar os endpoints, refinar o frontend e redigir textos.”

### 6. Encerramento
- **Fala:** “Com isso, demonstramos uma aplicação completa, com integração entre backend, banco de dados e frontend, atendendo aos requisitos propostos. Muito obrigado!”
