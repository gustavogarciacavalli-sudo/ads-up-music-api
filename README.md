# 🎵 BeatFlow - Sistema de Gestão Musical 🚀

**Disciplina:** Tópicos Especiais em Sistemas
**Professor:** Diogo Deconto

---

## 👥 Identificação do Projeto

- **Curso:** Análise e Desenvolvimento de Sistemas (ADS)
- **Integrantes:** 
  - Gustavo Henrique Garcia Cavalli
  - Pedro Henrique Policeno
  - Lucas Cardozo

---

## 📝 Resumo

O BeatFlow é um sistema completo de gestão musical desenvolvido com backend em .NET 8, banco de dados relacional SQLite e um frontend moderno construído em React Vanilla com TypeScript. O sistema permite o mapeamento e gerenciamento integrado de gêneros, artistas, músicas e playlists, criando um ecossistema musical interconectado e de fácil operação.

---

## ⚙️ Funcionalidades

- **Cadastro de gêneros musicais:** Gerenciamento das categorias musicais base.
- **Cadastro de artistas vinculados a gêneros:** Associação de cada artista ao seu respectivo gênero (Relacionamento 1:N).
- **Cadastro de músicas vinculadas a artistas:** Inserção de faixas com informações ricas como BPM e duração.
- **Criação de playlists:** Criação de coleções personalizadas de músicas.
- **Gestão de playlists (Adição/Remoção):** Inclusão ou exclusão flexível de músicas nas playlists.
- **Resumo da playlist:** Geração automática de estatísticas da playlist (BPM médio, duração total, contagem de faixas, diversidade de artistas e gêneros).
- **Dashboard gerencial:** Visão geral estatística do acervo musical no frontend.
- **Consumo de API:** Frontend React reativo consumindo os endpoints REST do backend.

---

## 🔍 Descrição das Funcionalidades

### 1. Gêneros
Permite definir a taxonomia base do sistema. Todo artista precisa estar vinculado a um gênero musical obrigatório para fins de categorização.

### 2. Artistas
Responsável pelo cadastro de talentos. O CRUD completo de artistas garante que biografias e nomes possam ser atualizados, refletindo em todo o ecossistema vinculado.

### 3. Músicas
O núcleo do sistema. As faixas cadastradas armazenam metadados valiosos (BPM e Duração) que são essenciais para os cálculos do sistema de playlists. Cada faixa possui um criador (Artista).

### 4. Playlists
A funcionalidade de negócio real do projeto. Em vez de apenas um cadastro simples, a playlist agrupa músicas e gera análises dinâmicas em tempo real, calculando o ritmo médio (BPM) das seleções para ajudar os usuários a criarem a "vibe" perfeita.

---

## 🛠️ Tecnologias Utilizadas

- **C#**
- **.NET 8**
- **Entity Framework Core**
- **SQLite**
- **Swagger**
- **React**
- **TypeScript**
- **React Router**
- **Axios / Fetch API**
- **CSS** (Vanilla)

---

## 🚀 Como rodar o backend

O backend deve ser executado a partir da pasta raiz do repositório onde está localizado o `.csproj`.

```bash
# 1. Restaure as dependências do .NET
dotnet restore

# 2. Compile e execute o servidor
dotnet run
```
O backend iniciará na porta configurada (geralmente `http://localhost:5270`). O banco de dados SQLite (`beatflow.db`) será criado automaticamente no startup e populado com dados iniciais se estiver vazio.

Para testar a API, acesse o Swagger: `http://localhost:5270/swagger`

---

## 💻 Como rodar o frontend

O frontend foi desenvolvido com Vite e deve ser executado a partir do diretório `frontend`.

```bash
# 1. Entre no diretório do frontend
cd frontend

# 2. Instale as dependências do Node.js
npm install

# 3. Inicie o servidor de desenvolvimento
npm run dev
```
O frontend ficará disponível em `http://localhost:5173`. 
*(Certifique-se de que o backend já está rodando para que os dados sejam carregados corretamente).*

---

## 🔗 Endpoints Principais

- **Gêneros:**
  - `GET /api/genres`
  - `POST /api/genres`
- **Artistas:**
  - `GET /api/artists`
  - `POST /api/artists`
- **Músicas:**
  - `GET /api/tracks`
  - `POST /api/tracks`
- **Playlists:**
  - `GET /api/playlists`
  - `POST /api/playlists`
  - `POST /api/playlists/{playlistId}/tracks/{trackId}` *(Adiciona música na playlist)*
  - `DELETE /api/playlists/{playlistId}/tracks/{trackId}` *(Remove música da playlist)*
  - `GET /api/playlists/{playlistId}/summary` *(Retorna estatísticas da playlist)*

---

## 🤖 Uso de IA

- **Ferramenta utilizada:** Antigravity (Google) com IA generativa (Gemini 1.5 Pro / Claude 3.5 Sonnet).
- **Forma de uso:** A inteligência artificial atuou como apoio pair-programmer na continuação do projeto base. Foi utilizada para a geração rápida do scaffolding do frontend em React TypeScript, refatoração e revisão dos endpoints do backend em .NET 8 (convertendo do .NET 10 original e removendo complexidades desnecessárias como AutoMapper para esta entrega), organização da estrutura de estilos CSS (Vanilla moderno) e auxílio na redação desta documentação e roteiro de apresentação.
- **Revisões realizadas pela equipe:** A equipe efetuou a verificação arquitetural do código sugerido, realizou os testes manuais end-to-end de integração entre o Frontend React e o Backend C#, certificou-se de que as 4 entidades propostas estavam corretas conforme especificado pelo professor, testou os cálculos de resumo da playlist, e garantiu a ausência de erros de CORS e compilação antes da gravação do vídeo final.
