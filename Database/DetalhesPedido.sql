CREATE TABLE [dbo].[DetalhesPedido] (
    [NroPedido] INT              NOT NULL,
    [ProdutoID] UNIQUEIDENTIFIER NULL,
    [Qtde]      INT              NULL,
    [Preco]     FLOAT (53)       NULL,
    PRIMARY KEY CLUSTERED ([NroPedido] ASC),
    CONSTRAINT [FK_DetalhesPedido_Pedidos] FOREIGN KEY ([NroPedido]) REFERENCES [dbo].[Pedidos] ([NroPedido]),
    CONSTRAINT [FK_DetalhesPedido_Produtos] FOREIGN KEY ([ProdutoID]) REFERENCES [dbo].[Produtos] ([ProdutoID])
);

