public class Fracao
{
    public int Numerador { get; private set; }
    public int Denominador { get; private set; }

    public Fracao(int numerador, int denominador)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(denominador, nameof(denominador));
        
        Numerador = numerador;
        Denominador = denominador;
        Simplificar();
    }
    
    public Fracao(int numerador) : this(numerador, 1) { }

    public Fracao(string fracaoString)
    {
        var partes = fracaoString.Split('/');
        if (partes.Length != 2)
            throw new ArgumentException("Use 'numerador/denominador'");
            
        if (!int.TryParse(partes[0], out int numerador) || !int.TryParse(partes[1], out int denominador))
            throw new ArgumentException("Valores numéricos inválidos");
            
        Numerador = numerador;
        Denominador = denominador;
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Denominador, nameof(Denominador));
        Simplificar();
    }
    
    public Fracao(double valor)
    {
        Denominador = 1;
        ConverterDoubleParaFracao(valor);
    }
    
    private void ConverterDoubleParaFracao(double valor)
    {
        const double epsilon = 1.0E-8;
        double n = Math.Abs(valor);
        int numerador = 1;
        int denominador = 1;
        double fracao = numerador / (double)denominador;
        
        while (Math.Abs(fracao - n) > epsilon)
        {
            if (fracao < n)
            {
                numerador++;
            }
            else
            {
                denominador++;
                numerador = (int)Math.Round(n * denominador);
            }
            fracao = numerador / (double)denominador;
        }
        
        Numerador = valor < 0 ? -numerador : numerador;
        Denominador = denominador;
        Simplificar();
    }
    
    private void Simplificar()
    {
        int mdc = MDC(Math.Abs(Numerador), Math.Abs(Denominador));
        Numerador /= mdc;
        Denominador /= mdc;
        
        if (Denominador < 0)
        {
            Numerador = -Numerador;
            Denominador = -Denominador;
        }
    }
    
    private static int MDC(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
    
    public Fracao Somar(Fracao outra)
    {
        int novoNumerador = Numerador * outra.Denominador + outra.Numerador * Denominador;
        int novoDenominador = Denominador * outra.Denominador;
        return new Fracao(novoNumerador, novoDenominador);
    }
    
    public Fracao Somar(int valor) => Somar(new Fracao(valor));
    public Fracao Somar(double valor) => Somar(new Fracao(valor));
    public Fracao Somar(string valor) => Somar(new Fracao(valor));
    
    public static Fracao operator +(Fracao a, Fracao b) => a.Somar(b);
    public static Fracao operator +(Fracao a, int b) => a.Somar(b);
    public static Fracao operator +(Fracao a, double b) => a.Somar(b);
    public static Fracao operator +(Fracao a, string b) => a.Somar(b);
    
    public override bool Equals(object obj)
    {
        if (obj is Fracao outra)
        {
            return Numerador == outra.Numerador && Denominador == outra.Denominador;
        }
        return false;
    }
    
    public override int GetHashCode() => HashCode.Combine(Numerador, Denominador);
    
    public static bool operator ==(Fracao a, Fracao b)
    {
        if (a is null) return b is null;
        if (b is null) return false;
        return a.Equals(b);
    }
    
    public static bool operator !=(Fracao a, Fracao b) => !(a == b);
    
    public static bool operator <(Fracao a, Fracao b)
    {
        double valorA = a.Numerador / (double)a.Denominador;
        double valorB = b.Numerador / (double)b.Denominador;
        return valorA < valorB;
    }
    
    public static bool operator >(Fracao a, Fracao b) => b < a;
    public static bool operator <=(Fracao a, Fracao b) => a < b || a == b;
    public static bool operator >=(Fracao a, Fracao b) => a > b || a == b;
    
    public bool IsPropria => Math.Abs(Numerador) < Denominador;
    public bool IsImpropria => Math.Abs(Numerador) >= Denominador;
    public bool IsAparente => Numerador % Denominador == 0;
    public bool IsUnitaria => Math.Abs(Numerador) == 1 && Denominador == 1;
    public override string ToString() => $"{Numerador}/{Denominador}";
}