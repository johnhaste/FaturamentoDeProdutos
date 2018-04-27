CREATE TABLE [dbo].[Enderecos] (
    [ClienteID]  UNIQUEIDENTIFIER NOT NULL,
    [EnderecoID] UNIQUEIDENTIFIER NOT NULL,
    [Rua]        VARCHAR (255)    NULL,
    [Cidade]     VARCHAR (255)    NULL,
    [Estado]     VARCHAR (255)    NULL,
    PRIMARY KEY CLUSTERED ([ClienteID] ASC),
    CONSTRAINT [FK_Enderecos_Clientes] FOREIGN KEY ([ClienteID]) REFERENCES [dbo].[Clientes] ([ClienteID])
);

