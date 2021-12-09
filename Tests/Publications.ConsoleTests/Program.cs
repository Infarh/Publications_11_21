using System.Linq.Expressions;

namespace Publications.ConsoleTests;

class Program
{
    public static void Main(string[] args)
    {
        //var visiter = new FileSystemVisiter();
        //var dir = new DirectoryInfo(@"..\..\..\..\..\UI\Publications.MVC");
        //var lines_count = visiter.CountSourceCodeLines(dir);
        //var source_files = visiter.GetSourceFiles(dir);
        //var source_files2 = dir.GetFiles("*.cs", SearchOption.AllDirectories);
        //Console.WriteLine("Количество строк кода: {0}", lines_count);

        Expression<Func<string, int>> string_len_calculator = str => str.Length;
        //var result = string_len_calculator("Hello World!");

        var function_str = string_len_calculator.Compile();
        var str_len = function_str("Hello World");

        Expression<Func<double, double>> calculator = x => Math.Sin(x) / x;
        var calculator_function = calculator.Compile();
        var result = calculator_function(0);

        var parameter_a = Expression.Parameter(typeof(int), "a");
        var parameter_b = Expression.Parameter(typeof(int), "b");

        var sum = Expression.Add(parameter_a, parameter_b);
        var product = Expression.Multiply(parameter_a, parameter_b);

        var expression_visiter = new ReplaceVisitor(parameter_a, product);

        var sum2 = expression_visiter.Visit(sum);

        var summator_expr = Expression.Lambda<Func<int, int, int>>(sum2, parameter_a, parameter_b);

        var summator = summator_expr.Compile();
        var summ_result = summator(5, 7);
    }
}