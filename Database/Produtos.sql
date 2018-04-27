CREATE TABLE [dbo].[Produtos] (
    [ProdutoID] UNIQUEIDENTIFIER NOT NULL,
    [Descricao] VARCHAR (255)    NULL,
    [Preco]     FLOAT (53)       NULL,
    PRIMARY KEY CLUSTERED ([ProdutoID] ASC)
);

