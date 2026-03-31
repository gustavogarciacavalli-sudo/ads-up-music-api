# 🎵 BeatFlow API - Gestão Musical Pro 🚀

**Projeto Final – Parte 1** | **Disciplina:** Tópicos Especiais em Sistemas (2026.01)
**Professor:** Diogo Deconto

---

## 👥 Identificação do Projeto

| Campo        | Informação                              |
|--------------|-----------------------------------------|
| **Projeto**  | BeatFlow API                            |
| **Curso**    | Análise e Desenvolvimento de Sistemas (ADS) |
| **Integrantes** | Gustavo Henrique Garcia Cavalli      |
|              | Pedro Henrique Policeno                 |
|              | Pedro Henrique Frason                   |
|              | Lucas Cardozo                           |

---

## 📝 Resumo

> *Escrito com o auxílio de IA (Gemini 1.5 Pro)*

O BeatFlow API é uma solução de backend desenvolvida para centralizar a gestão de ecossistemas musicais.
Utilizando uma arquitetura moderna baseada em Minimal APIs do .NET 8, o sistema permite o mapeamento
completo de artistas e sua classificação por gêneros musicais. O projeto foca na integridade de dados
através de relacionamentos entre entidades e persistência via SQLite, oferecendo uma interface robusta
para desenvolvedores via Swagger e uma experiência visual nostálgica para o usuário final, unindo a
eficiência técnica do C# com a estética "Vibe Coding" da Web 1.0.

---

## ⚙️ Funcionalidades

> *Texto produzido com auxílio de IA*

- 🎸 **Gestão de Gêneros Musicais** — Cadastro e listagem de categorias (Rock, Trap, Hip-Hop, etc.)
- 👤 **CRUD Completo de Artistas** — Inserção, leitura, atualização e exclusão de talentos
- 🔗 **Vínculo Estruturado** — Associação obrigatória de cada artista a um gênero pré-existente (Relacionamento 1:N)
- 🌈 **Interface Retro-Futurista** — Front-end funcional com rastro arco-íris e elementos visuais dos anos 2000
- 📄 **Documentação Automática** — Endpoint interativo via Swagger para testes de integração

---

## 🔍 Descrição das Funcionalidades

> *Redação elaborada com auxílio de IA*

### 1. Módulo de Gêneros (Relacionamento)
O sistema exige que um Gênero seja cadastrado antes do Artista. Isso garante a organização taxonômica
do banco de dados, permitindo filtragens e buscas por categorias específicas.

### 2. Operações CRUD de Artistas
O usuário pode realizar o ciclo completo de gerenciamento:
- Cadastrar um novo artista vinculado a um gênero
- Listar todos os registros em uma tabela dinâmica
- Editar informações de artistas existentes
- Remover registros obsoletos diretamente pela interface

### 3. Experiência de Usuário (Vibe Coding)
A camada visual utiliza Vanilla JavaScript para consumir a API em tempo real. Implementamos efeitos
psicodélicos (**Mouse Trail Rainbow**) e animações senoidais (**Cobra Wave**) para criar uma identidade
visual única que remete à era Geocities.

### 4. Persistência com Entity Framework
Toda a lógica de dados é mediada pelo EF Core, que gerencia a criação do banco SQLite e as relações
de chave estrangeira entre Artistas e Gêneros de forma automática.

---

## 🛠️ Tecnologias Utilizadas

| Camada          | Tecnologia                                        |
|-----------------|---------------------------------------------------|
| **Backend**     | C# (.NET 8 Minimal API)                           |
| **ORM**         | Entity Framework Core                             |
| **Banco de Dados** | SQLite                                         |
| **Frontend**    | HTML5, CSS3 (Vibe Coding Style), JavaScript (Fetch API) |
| **Versionamento** | GitHub                                          |

---

## 🚀 Como Rodar o Projeto

### Pré-requisitos

- SDK do **.NET 8.0** instalado
- **Git** configurado

### 1. Clonar o Repositório
```bash
git clone https://github.com/gustavogarciacavalli-sudo/ads-up-music-api.git
cd ads-up-music-api/BeatFlowApi
```

### 2. Restaurar Dependências e Executar
```bash
# Restaura dependências e instala pacotes (EF/Swagger)
dotnet restore

# Executa a aplicação
dotnet run
```

### 3. Links de Acesso

| Página       | URL                                  |
|--------------|--------------------------------------|
| 🏠 Home      | http://localhost:5000                |
| 🎤 Artistas  | http://localhost:5000/artists-page   |
| 📄 Swagger   | http://localhost:5000/swagger        |

---

## 🤖 Uso de IA

| Campo               | Detalhe                                                                 |
|---------------------|-------------------------------------------------------------------------|
| **Ferramenta**      | Gemini 3 Flash (Paid Tier)                                              |
| **Forma de uso**    | Redação técnica das seções de Resumo, Funcionalidades e Descrições; estruturação dos endpoints REST; lógica de estilização do rastro arco-íris via Vibe Coding |
| **Revisões**        | A equipe revisou os textos gerados para garantir que as funcionalidades descritas correspondiam à implementação em C#, ajustando detalhes do relacionamento entre entidades e a indentação do README |
