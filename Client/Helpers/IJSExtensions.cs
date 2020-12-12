using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apoyo.Client.Helpers
{
    public static class IJSExtensions
    {
        public static ValueTask<object> SetInLocalStorage(this IJSRuntime js, string key, string content) => js.InvokeAsync<object>("localStorage.setItem", key, content);

        public static ValueTask<string> GetFromLocalStorage(this IJSRuntime js, string key) => js.InvokeAsync<string>("localStorage.getItem", key);

        public static ValueTask<object> RemoveItem(this IJSRuntime js, string key) => js.InvokeAsync<object>("localStorage.removeItem", key);

        public static ValueTask<object> clickMenu(this IJSRuntime js) {
            return js.InvokeAsync<object>("menuClick", null);
        }

        public static ValueTask<object> hover(this IJSRuntime js)
        {
            return js.InvokeAsync<object>("hoverImages", null);
        }


        public static ValueTask<object> MostrarMensaje(this IJSRuntime js, string mensaje)
        {
            return js.InvokeAsync<object>("Swal.fire", mensaje);
        }
        public static ValueTask<object> MostrarMensaje(this IJSRuntime js, string titulo, string mensaje, TipoMensajeSweetAlert tipo)
        {
            return js.InvokeAsync<object>("Swal.fire", titulo, mensaje, tipo.ToString());
        }
        public async static Task<bool> Confirm(this IJSRuntime js, string titulo, string mensaje, TipoMensajeSweetAlert tipo) {
            return await js.InvokeAsync<bool>("CustomConfirm", titulo, mensaje, tipo);
        }


    }
    public enum TipoMensajeSweetAlert
    {
        question, warning, error, success, info
    }
}
