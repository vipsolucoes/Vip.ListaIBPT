using FileHelpers;

namespace Vip.ListaIBPT;

[DelimitedRecord(";")]
[IgnoreFirst(1)]
public class Ncm : IEquatable<Ncm>
{
    #region Propriedades

    public string Codigo { get; set; }
    public string Ex { get; set; }
    public int Tipo { get; set; }
    [FieldQuoted] public string Descricao { get; set; }

    [FieldConverter(ConverterKind.Decimal, ".")]
    public decimal NacionalFederal { get; set; }

    [FieldConverter(ConverterKind.Decimal, ".")]
    public decimal ImportadoFederal { get; set; }

    [FieldConverter(ConverterKind.Decimal, ".")]
    public decimal Estadual { get; set; }

    [FieldConverter(ConverterKind.Decimal, ".")]
    public decimal Municipal { get; set; }

    [FieldConverter(ConverterKind.Date, "dd/MM/yyyy")]
    public DateTime VigenciaInicio { get; set; }

    [FieldConverter(ConverterKind.Date, "dd/MM/yyyy")]
    public DateTime VigenciaFim { get; set; }

    public string Chave { get; set; }
    public string Versao { get; set; }
    public string Fonte { get; set; }

    #endregion

    #region Métodos

    public bool Equals(Ncm other)
    {
        return Codigo == other?.Codigo;
    }

    public override int GetHashCode()
    {
        return Codigo.GetHashCode();
    }

    #endregion
}