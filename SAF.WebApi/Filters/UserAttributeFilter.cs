using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SAF.WebApi.Filters
{
    /// <summary>
    /// Garante a existência da Claim 'PessoaId' no Request e limita o acesso a determinada rota ao perfil esperado (PessoaTipo)
    /// </summary>
    public class UserAttributeFilter : Attribute, IAsyncActionFilter
    {
        readonly string _pessoaTipo;

        public UserAttributeFilter(object[] Parameters)
        {
            _pessoaTipo = Parameters[0].ToString();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var pessoaId = context.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "PessoaId")?.Value;
            var pessoaTipo = context.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "PessoaTipo")?.Value;

            if (string.IsNullOrEmpty(pessoaId) ||
                string.IsNullOrEmpty(pessoaTipo) || 
                !pessoaTipo.Equals(_pessoaTipo))
            {
                context.Result = new BadRequestObjectResult("Requisição não autorizada.");

                return;
            }

            await next();
        }
    }
}