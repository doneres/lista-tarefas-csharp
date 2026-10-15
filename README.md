# Lista de Tarefas (To-Do) — C# Windows Forms + SQLite

Aplicação desktop para controlar tarefas: cadastro com datas de início e término,
marcação de conclusão, reordenação por importância e destaque das tarefas atrasadas.

## Como executar

1. Abra `ListaTarefas.sln` no Visual Studio 2022 (workload ".NET desktop development").
2. Compile (Ctrl+Shift+B). Os pacotes NuGet são restaurados automaticamente:
   - `Microsoft.Data.Sqlite`
   - `System.Configuration.ConfigurationManager`
3. Execute (F5). O banco `listatarefas.db` é criado sozinho na pasta do executável
   na primeira execução; os dados ficam salvos entre execuções.

## Estrutura (separação de camadas)

```
ListaTarefas/
├── App.config                 connection string (fora do código-fonte)
├── Program.cs                 inicialização, criação do banco, tratamento global de erros
├── Models/Tarefa.cs           entidade + regra "está atrasada"
├── Data/                      acesso a dados (único lugar com SQL)
│   ├── ConexaoFactory.cs      lê a connection string do App.config
│   ├── BancoDeDados.cs        cria a tabela se não existir
│   ├── TarefaRepository.cs    CRUD parametrizado + transações
│   └── AcessoDadosException.cs
├── Services/                  regras de negócio
│   ├── TarefaService.cs       validações, conclusão, reordenação
│   └── RegraNegocioException.cs
├── Infra/RegistroErros.cs     log técnico em erros.log
└── UI/                        formulários (só interface)
    ├── FormPrincipal          lista, destaque de atrasadas, ações
    └── FormTarefa             cadastro/edição com validação de formato
```

Fluxo: **Form → TarefaService → TarefaRepository → SQLite**. Nenhum formulário contém SQL.

## Requisitos da proposta

| Requisito | Onde |
|---|---|
| Dados salvos | SQLite local (`listatarefas.db`) |
| Data para iniciar e terminar | `FormTarefa`: campos com máscara, opcionais |
| Tela inicial lista as tarefas por fazer | `FormPrincipal` carrega só pendentes (opção "Mostrar concluídas") |
| Marcar como concluída facilmente | Botão Concluir ou Ctrl+Enter; grava a data de conclusão |
| Trocar a ordem facilmente | Botões Subir/Descer ou Ctrl+↑ / Ctrl+↓ direto na lista |
| Atrasadas em destaque | Linha em vermelho, negrito, situação "ATRASADA" |

## Critérios de avaliação

- **Separação de camadas:** UI, Services e Data em pastas/classes distintas; formulários só chamam o `TarefaService`.
- **Acesso a dados:** conexões criadas e descartadas com `using` em cada operação; todo SQL usa parâmetros (`@id`, `@titulo`...); `TrocarOrdem` e `Inserir` rodam em transação (as duas atualizações de ordem acontecem juntas ou nenhuma).
- **Validação:** na interface — `ErrorProvider`, `MaskedTextBox` (dd/mm/aaaa) e `DateTime.TryParseExact`; no negócio — título obrigatório e tamanho máximo, término não anterior ao início, só pendentes podem ser reordenadas.
- **Exceções:** `SqliteException` é capturada no repositório e convertida em `AcessoDadosException` com mensagem amigável; a pilha vai para `erros.log`, nunca para a tela. Nenhum `catch` vazio genérico. Erros inesperados passam por um tratador global.
- **Configuração:** connection string em `App.config`; SQLite local, sem usuário/senha.
- **Teclado:** foco inicial na lista / no campo Título; `TabIndex` em ordem; teclas de acesso (Alt+letra sublinhada); atalhos Ctrl+N, Enter, Ctrl+Enter, Ctrl+↑/↓, Delete, F5; Enter salva e Esc cancela no cadastro.
- **Código:** prefixos de controle (`btn`, `txt`, `mtb`, `dgv`, `chk`, `lbl`, `err`), nomes em português coerentes, uma responsabilidade por classe.

## Atalhos

| Tecla | Ação |
|---|---|
| Ctrl+N | Nova tarefa |
| Enter / duplo clique | Editar tarefa selecionada |
| Ctrl+Enter | Concluir / reabrir |
| Ctrl+↑ / Ctrl+↓ | Mover tarefa para cima / para baixo |
| Delete | Excluir (com confirmação) |
| F5 | Recarregar lista |
