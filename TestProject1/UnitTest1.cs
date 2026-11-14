
using System.ComponentModel.DataAnnotations;
using OnixSystem_MVC.Models.Entidades;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void CadastrarProduto_SemNome_DeveImpedirCadastro()
        {
            // Arrange - Preparar um produto sem nome
            var produto = new Produto
            {
                Nome = "",
                Tamanho = "M",
                Detalhes = "Camiseta básica",
                Preco = 59.90m,
                CodigoBarras = "123456789"
            };

            // Act - Executar a validação
            var resultados = new List<ValidationResult>();

            var valido = Validator.TryValidateObject(
                produto,
                new ValidationContext(produto),
                resultados,
                validateAllProperties: true
            );

            // Assert - Verificar o resultado esperado
            Assert.False(valido);

            Assert.Contains(resultados, erro =>
                erro.ErrorMessage == "O nome é obrigatório.");
        }
    }
}
