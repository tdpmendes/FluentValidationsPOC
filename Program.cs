using FluentValidation.Results;
using FluentValidationsPOC.Models;
using FluentValidationsPOC.Validation;
using System;

namespace FluentValidationsPOC
{
    class Program
    {
        static void Main(string[] args)
        {
            TheModel m = new TheModel();
            
            Console.Write("Enter CPF: ");
            m.Cpf = Console.ReadLine();
            Console.Write("Enter CNPJ: ");
            m.Cnpj = Console.ReadLine();

            Console.WriteLine("Validating the model cpf: " + m.Cpf);
            Console.WriteLine("Validating the model cnpj: " + m.Cnpj);

            TheModelValidation val = new TheModelValidation();

            ValidationResult r = val.Validate(m);

            foreach( var item in r.Errors)
            {
                Console.WriteLine("CODE: " + item.ErrorCode + " - ERROR: " + item.ErrorMessage);
            }
        }
    }
}
