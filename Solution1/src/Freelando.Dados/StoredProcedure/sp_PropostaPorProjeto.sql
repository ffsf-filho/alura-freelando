USE [FreelandoVX]
GO

/****** Object:  StoredProcedure [dbo].[sp_PropostaPorProjeto]    Script Date: 19/01/2025 02:27:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_PropostaPorProjeto] 
@Id_Projeto uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id_Proposta, Id_Projeto, Id_Profissional, Valor_Proposta, Data_Proposta, Mensagem, Prazo_Entrega
	FROM dbo.TB_Propostas
	WHERE Id_Projeto = @Id_Projeto
END
GO


