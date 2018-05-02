CREATE TABLE [dbo].[DetalhesPedido] (
    [NroPedido] INT              NOT NULL,
    [ProdutoID] UNIQUEIDENTIFIER NOT NULL,
    [Qtde]      INT              NOT NULL,
    [Preco]     FLOAT (53)       NOT NULL,
    PRIMARY KEY CLUSTERED ([NroPedido], [ProdutoID]),
    CONSTRAINT [FK_DetalhesPedido_Pedidos] FOREIGN KEY ([NroPedido]) REFERENCES [dbo].[Pedidos] ([NroPedido]),
    CONSTRAINT [FK_DetalhesPedido_Produtos] FOREIGN KEY ([ProdutoID]) REFERENCES [dbo].[Produtos] ([ProdutoID])
);

