namespace MyFirstLang
{

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write(">> ");

                var line = Console.ReadLine();
                var lexer = new Lexer(line);

                if (string.IsNullOrWhiteSpace(line))
                    return; 

                while(true)
                {
                    var token = lexer.nextToken();

                    if (token.Kind == SyntaxKind.EndOfFileToken)
                        break;

                    Console.Write($"{token.Kind}: '{token.Text}'");
                    if(token.Value != null)
                        Console.Write($" {token.Value}");
                    Console.WriteLine();
                    
                }
            }
        }
    }

    enum SyntaxKind
    {
        ExpressionToken,
        NumberToken,
        WhitespaceToken,
        OpenBracketToken,
        CloseBracketToken,
        ForeSlashToken,
        StarToken,
        PlusToken,
        DashToken,
        BadToken,
        EndOfFileToken
    }

    class SyntaxToken
    {
        public SyntaxToken( string text, int position, SyntaxKind kind, Object value )
        
        {
            Text = text;
            Position = position;
            Kind = kind;
            Value = value;
        }

        public string Text { get; }
        public int Position { get; }
        public SyntaxKind Kind { get; }
        public object Value { get; }
    }
    class Lexer
    {

        private readonly string _text;
        private int _pos;
        public Lexer(string text)
        {
            // Console.WriteLine($"Line: {text}, with length {text.Length}");
            _text = text; 
        }

        private char Current
        {
            get
            {
                if(_pos >= _text.Length)
                    return '\0';

                return _text[_pos];
            }
            
        }

        private void Next()
        {
            Console.WriteLine($"Current position: {_pos}");

            _pos++;

            // Console.WriteLine($"Next position: {_pos}");
        }

        public SyntaxToken nextToken()
        {
            // check for EndOfFile
            if (_pos >= _text.Length)
                return new SyntaxToken("\0", _pos, SyntaxKind.EndOfFileToken, null);

            // what we are loking for are: <numbers>, + - / * ( ), <whitespace>
            if (char.IsDigit(Current)) 
            {
                int start = _pos;

                while(char.IsDigit(Current))
                    Next();
                
                int length = _pos - start;
                var text = _text.Substring(start, length);
                int.TryParse(text, out var value);

                return new SyntaxToken(text, start, SyntaxKind.NumberToken, value);
            }
            
            if (char.IsWhiteSpace(Current)) 
            {
                int start = _pos;

                while(char.IsWhiteSpace(Current))
                    Next();
                
                int length = _pos - start;
                var text = _text.Substring(start, length);
                int.TryParse(_text, out var value);

                return new SyntaxToken(text, start, SyntaxKind.WhitespaceToken, null);
            }

            if(Current == '+')
                return new SyntaxToken("+", _pos++, SyntaxKind.PlusToken, null);

            else if(Current == '-')
                return new SyntaxToken("-", _pos++, SyntaxKind.DashToken, null);

            else if(Current == '*')
                return new SyntaxToken("*", _pos++, SyntaxKind.StarToken, null);

            else if(Current == '/')
                return new SyntaxToken("/", _pos++, SyntaxKind.ForeSlashToken, null);

            else if(Current == '(')
                return new SyntaxToken("(", _pos++, SyntaxKind.OpenBracketToken, null);

            else if(Current == ')')
                return new SyntaxToken(")", _pos++, SyntaxKind.CloseBracketToken, null);

            return new SyntaxToken(_text.Substring(_pos - 1, 1), _pos++, SyntaxKind.BadToken, null);
        }
    }
}