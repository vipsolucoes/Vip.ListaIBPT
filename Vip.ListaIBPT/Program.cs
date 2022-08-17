using System.Globalization;
using System.Text;
using FileHelpers;
using Vip.ListaIBPT;

Console.ForegroundColor = ConsoleColor.DarkGreen;
ImprimeLinha("Projeto para converter lista IBPT de csv para instrução SQL", 2);
Console.ForegroundColor = ConsoleColor.White;

ImprimeLinha("Informe o caminho do arquivo CSV");
Console.Write("> ");
var caminhoArquivo = Console.ReadLine();

if (caminhoArquivo.IsNullOrEmpty() || !File.Exists(caminhoArquivo))
{
    ImprimeVermelho($"Arquivo {caminhoArquivo} não encontrado! Por favor, tente novamente");
    return;
}

ImprimeVerde($"Arquivo {caminhoArquivo} encontrado com sucesso!", 2);
ImprimeLinha("Aguarde um momento... arquivo será processado!");

// é necessário converter o arquivo original para UTF8
var engine = new FileHelperEngine<Ncm>(Encoding.UTF8);
var registros = engine.ReadFile(caminhoArquivo).Where(x => x.Tipo == 0 && x.Codigo.Length == 8).Distinct().ToList();

if (registros.IsNull() || registros.IsEmpty())
{
    ImprimeVermelho("Nenhuma informação encontrada. Tente novamente");
    return;
}

const int totalLinhas = 240;
var linha = 1;
var totalArquivo = 1;

var arquivo = new StringBuilder();
arquivo.AppendLine(MontarCabecalho());
foreach (var registro in registros)
{
    arquivo.AppendLine(MontarLinha(registro, linha.Equals(totalLinhas) || totalArquivo.Equals(registros.Count)));

    if (linha.Equals(totalLinhas))
    {
        arquivo.AppendLine(MontarCabecalho());
        linha = 1;
    }

    linha++;
    totalArquivo++;
}

File.WriteAllText(@"D:\tabela.sql", arquivo.ToString());
ImprimeVerde($"Arquivo convertido com sucesso! Total de registros: {registros.Count}");

Console.ReadKey();

#region Funções

string MontarCabecalho()
{
    return "INSERT INTO \"Ncm\" (\"NcmId\", \"Descricao\", \"ImpostoFederalNacional\", \"ImpostoFederalImportado\", \"ImpostoEstadual\", \"ImpostoMunicipal\", \"Inativo\") VALUES ";
}

string MontarLinha(Ncm ncm, bool ultimaLinha)
{
    var numberFormat = new NumberFormatInfo {NumberDecimalSeparator = "."};

    var registro = $"('{ncm.Codigo}','{ncm.Descricao}',{ncm.NacionalFederal.ToString(numberFormat)},{ncm.ImportadoFederal.ToString(numberFormat)},{ncm.Estadual.ToString(numberFormat)},{ncm.Municipal.ToString(numberFormat)},0)";
    registro += ultimaLinha ? "; \r\n" : ",";
    return registro;
}

void ImprimeLinha(string texto, int pularLinha = 0)
{
    Console.WriteLine(texto);

    if (pularLinha > 0)
        for (var i = 0; i < pularLinha; i++)
            Console.WriteLine(string.Empty);
}

void ImprimeVerde(string texto, int pularLinha = 1)
{
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    ImprimeLinha(texto, pularLinha);
    Console.ForegroundColor = ConsoleColor.White;
}

void ImprimeVermelho(string texto, int pularLinha = 1)
{
    Console.ForegroundColor = ConsoleColor.Red;
    ImprimeLinha(texto, pularLinha);
    Console.ForegroundColor = ConsoleColor.White;
}

#endregion