# Contribuindo com BeatFlow API

Este guia explica como rodar e contribuir para o projeto localmente.

## Requisitos
- Docker e Docker Compose instalados.

## Como rodar localmente
1. Clone o repositório.
2. Na raiz do projeto, execute o comando:
   ```bash
   docker-compose up --build
   ```
3. A API estará disponível na porta 8080.
4. Para acessar a documentação (Swagger), navegue até `http://localhost:8080/swagger`.

## Padrão de Branch e Commits
1. Crie uma branch baseada na `main` no formato `feat/nome-da-tarefa`.
2. Após codar, adicione e faça commit com mensagens claras (`feat: ...` ou `fix: ...`).
3. Dê push para o repositório remoto.
