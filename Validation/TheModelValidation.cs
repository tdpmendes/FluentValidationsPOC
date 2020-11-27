using FluentValidation;
using FluentValidationsPOC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace FluentValidationsPOC.Validation
{
    public class TheModelValidation : AbstractValidator<TheModel>
    {
        private ResourceManager rm { get; set; }

        public TheModelValidation()
        {
            rm = new ResourceManager("FluentValidationsPOC.Properties.ValidationMessages",Assembly.GetExecutingAssembly());

            RuleFor(x => x.Cpf).Must(ValidateCPF).WithErrorCode("001").WithMessage(rm.GetString("001"));
            RuleFor(x => x.Cnpj).Must(ValidateCNPJ).WithErrorCode("002").WithMessage(rm.GetString("002"));
            //Aqui na realidade tantofaz
            //pq na realidade a gente vai 
            //validar o model inteiro
            RuleFor(x => x.Cpf).Must((model,campo) => ValidateEntireModel(model)).WithErrorCode("003").WithMessage(rm.GetString("003"));
        }

        private bool ValidateEntireModel(TheModel m)
        {
            return !string.IsNullOrWhiteSpace(m.Cpf) && !string.IsNullOrWhiteSpace(m.Cnpj);
        }

        public bool ValidateCPF(string cpf)
        {
            if (cpf == null) return false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11) return false;

            tempCpf = cpf.Substring(0, 9);

            soma = 0;
            for (int i = 0; i < 9; i++) soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            
            resto = soma % 11;
            
            if (resto < 2) resto = 0;
            else resto = 11 - resto;

            digito = resto.ToString();
            tempCpf = tempCpf + digito;

            soma = 0;
            for (int i = 0; i < 10; i++) soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            
            resto = soma % 11; 
            
            if (resto < 2) resto = 0;
            else resto = 11 - resto;
            
            digito = digito + resto.ToString();
            
            return cpf.EndsWith(digito);
        }

        public bool ValidateCNPJ(string cnpj)
        {
            if (cnpj == null) return false;

            try
            {
                int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

                int soma;
                int resto;
                string digito;
                string tempCnpj;

                cnpj = cnpj.Trim();
                cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

                if (cnpj.Length != 14) return false;

                tempCnpj = cnpj.Substring(0, 12);
                soma = 0;

                for (int i = 0; i < 12; i++) soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
                resto = (soma % 11); 
                
                if (resto < 2) resto = 0;
                else resto = 11 - resto;

                digito = resto.ToString();
                tempCnpj = tempCnpj + digito;
                soma = 0;

                for (int i = 0; i < 13; i++) soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
                resto = (soma % 11);

                if (resto < 2) resto = 0;
                else resto = 11 - resto;

                digito = digito + resto.ToString();
                return cnpj.EndsWith(digito);
            }
            catch (Exception) { return false; }
        }
    }
}
