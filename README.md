# 🎵 BeatFlow API - Gestão de Catálogo Musical

O **BeatFlow** é uma API de backend robusta projetada para gerenciar catálogos musicais, gravadoras independentes ou beatstores. O sistema foca na organização de obras fonográficas, permitindo o controle total sobre artistas, faixas e gêneros musicais através de uma arquitetura moderna e escalável.

Este projeto foi desenvolvido como parte da disciplina de **Tópicos Especiais em Sistemas** na Universidade Positivo, aplicando conceitos avançados de desenvolvimento em C# e persistência de dados.

---

## 🚀 Tecnologias e Arquitetura

O projeto utiliza as ferramentas mais atuais do ecossistema .NET para garantir alta performance e código limpo:

* **Linguagem:** C# (SDK 8.0)
* **Framework:** ASP.NET Core (Minimal APIs para rotas de alta velocidade)
* **ORM:** Entity Framework Core (Abstração e mapeamento de dados)
* **Banco de Dados:** SQLite (Persistência leve e portátil)
* **Versionamento:** Git & GitHub (Fluxo de colaboração em equipe)

---

## 🛠️ Funcionalidades Principais

* **Gestão de Artistas:** Registro completo de produtores e músicos, servindo como base para o catálogo.
* **Catálogo de Faixas (CRUD):** Sistema completo de criação, leitura, atualização e deleção de músicas e beats.
* **Relacionamento Relacional:** Implementação de chaves estrangeiras para vincular automaticamente as faixas aos seus respectivos autores (Relação 1:N).
* **Integração de Dados:** Endpoints REST que processam e retornam objetos JSON padronizados.
* **Persistência Segura:** Uso de migrações (Migrations) para manter a integridade do banco de dados SQLite.

---

## 📂 Como Rodar o Projeto

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/](https://github.com/)[SEU-USUARIO]/beat-flow-api.git
    ```
2.  **Acesse a pasta do projeto:**
    ```bash
    cd beat-flow-api
    ```
3.  **Execute a aplicação:**
    ```bash
    dotnet run
    ```
4.  **Teste a API:** A documentação Swagger (se habilitada) estará disponível em `http://localhost:[PORTA]/swagger` ou utilize ferramentas como Postman/Insomnia nos endpoints configurados.

---

## 👥 Desenvolvedores
* **Gustavo Henrique** - *Desenvolvedor Backend*
* [Nome do Colega 2] - *Desenvolvedor Backend*
* [Nome do Colega 3] - *Desenvolvedor Backend*

---

## 🤖 Uso de Inteligência Artificial

Conforme as diretrizes pedagógicas da disciplina, este projeto utilizou ferramentas de IA em seu desenvolvimento:

* **Ferramenta:** Gemini (Google).
* **Aplicação:** A IA foi utilizada para a estruturação técnica deste README, auxílio na padronização das descrições das Minimal APIs e refinamento da redação técnica para fins de portfólio.
* **Revisão Humana:** Toda a documentação e lógica de código foram revisadas e validadas pelos integrantes do grupo para garantir que o conteúdo reflete exatamente a implementação técnica realizada.

---
