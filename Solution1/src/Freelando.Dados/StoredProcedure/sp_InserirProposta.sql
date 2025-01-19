USE [FreelandoVX]
GO

/****** Object:  StoredProcedure [dbo].[sp_InserirProposta]    Script Date: 19/01/2025 02:27:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_InserirProposta]
  @Id_Proposta uniqueidentifier,
  @Id_Projeto uniqueidentifier,
  @Id_Profissional uniqueidentifier,
  @Valor_Proposta decimal(8,2),
  @Prazo_Entrega date,
  @Mensagem nvarchar(50)
AS
BEGIN
  SET NOCOUNT ON;

  BEGIN TRY
    BEGIN TRANSACTION;

    INSERT INTO dbo.TB_Propostas
      (Id_Proposta, Id_Projeto, Id_Profissional, Valor_Proposta, Prazo_Entrega, Mensagem)
    VALUES
      (@Id_Proposta, @Id_Projeto, @Id_Profissional, @Valor_Proposta, @Prazo_Entrega, @Mensagem);

    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    ROLLBACK TRANSACTION;
    -- Manejo do erro, lançando a exceção capturada
    THROW;
  END CATCH;
END;
GO


