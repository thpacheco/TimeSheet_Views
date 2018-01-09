using System;

namespace TimeSheet.Forms.Util
{
    public static class ConverteHoras
    {
        public static TimeSpan ConverterHoraMinutos(string horario)
        {
            int horarioHora = Convert.ToInt32(horario.Substring(0, 2));
            int horarioMinuto = Convert.ToInt32(horario.Substring(3, 2));

            TimeSpan horarioFinal = new TimeSpan(horarioHora, horarioMinuto, 0);

            return horarioFinal;
        }
    }
}
