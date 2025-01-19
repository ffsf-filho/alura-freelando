USE [FreelandoVX]
GO

/****** Object:  StoredProcedure [dbo].[sp_PropostaSummary]    Script Date: 19/01/2025 02:28:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_PropostaSummary]
AS
BEGIN
    SELECT Id_Proposta, Data_Proposta, Prazo_Entrega
    FROM dbo.TB_Propostas
END
GO


