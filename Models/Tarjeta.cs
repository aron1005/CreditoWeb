using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CreditoWeb.Models
{
    public class Tarjeta
    {
        [Required(ErrorMessage = "El número de tarjeta es necesario.")]
        [CreditCard]
        public string TarjetaNum { get; set; }
        public TipoTarjeta TipoTarjeta { get; set; }

        public bool Valida { get; set; }

        public Tarjeta(string tarjetaNum)
        {
            this.TarjetaNum = tarjetaNum;
            Valida = esValida();
            TipoTarjeta = tipoDeTarjeta();
        }


        /// Basado en el algoritmo de Luhn determinar si esta tarjeta es valida
        /// como estamos dentro de la clase de tarjeta tenemos acceso a la propiedad TarjetaNum 
        private bool esValida()
        {
            return TarjetaNum.Reverse() // ordena el número de la tarjeta al revés 
                               .Select(c => char.GetNumericValue(c)) // convierte los caracteres a un valor numérico
                               .Select((num, index) => index % 2 == 0 ? num : ((num *= 2) > 9 ? num - 9 : num)) // si la posición es par lo multiplica por dos y suma los digitos 
                               .Sum() % 10 == 0; // suma todos los elementos de la lista y pregutna si es multiplo de 10 sacando el residuo de la operación

        }


        /// Si la tarjeta es valida determinar de cuál tipo es VISA, MASTERCARD, AMERICANEXPRESS
        /// como estamos dentro de la clase de tarjeta tenemos acceso a la propiedad TarjetaNum 
        private TipoTarjeta tipoDeTarjeta()
        {
            if (Valida)
            {
                var longitud = TarjetaNum.Length;
                var primerosDos = TarjetaNum.Substring(0, 2);

                if (longitud == 15 && (primerosDos.Equals("34") || primerosDos.Equals("37")))
                {
                    return TipoTarjeta.AMERICANEXPRESS;
                }
                if (longitud == 16)
                {
                    if (primerosDos.Equals("51") || primerosDos.Equals("52") || primerosDos.Equals("53") || primerosDos.Equals("54") || primerosDos.Equals("55"))
                    {
                        return TipoTarjeta.MASTERCARD;
                    }
                    if (primerosDos.StartsWith("4"))
                    {
                        return TipoTarjeta.VISA;
                    }
                }
                return TipoTarjeta.NOVALIDA; // Si es valida pero no es de ningún tipo
            }
            return TipoTarjeta.NOVALIDA;
        }



    }

    public enum TipoTarjeta
    {
        VISA,
        MASTERCARD,
        AMERICANEXPRESS,
        NOVALIDA


    }
}