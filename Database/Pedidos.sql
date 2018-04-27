CREATE TABLE [dbo].[Pedidos] (
    [ClienteID] UNIQUEIDENTIFIER NOT NULL,
    [NroPedido] INT              NOT NULL,
    [Data]      DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([NroPedido] ASC),
    CONSTRAINT [FK_Pedidos_Clientes] FOREIGN KEY ([ClienteID]) REFERENCES [dbo].[Clientes] ([ClienteID])
);

