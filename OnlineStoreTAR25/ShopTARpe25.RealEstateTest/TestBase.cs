using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.RealEstateTest.Macros;


namespace ShopTARpe25.RealEstateTest
{
    public abstract class TestBase
    {
        // Siia salvestatakse testide jaoks vajalikud teenused.
        protected IServiceProvider serviceProvider { get; set; }

        protected TestBase()
        {
            // Loob uue teenuse kogu vajalikuks teenuse testimiseks.
            var services = new ServiceCollection();
            // Kutsub välja meetodi.
            SetupServices(services);
            // Teeb teenused kasutamiseks valmis.
            serviceProvider = services.BuildServiceProvider();
        }

        public virtual void SetupServices(IServiceCollection services)
        {
            // Kui küsitakse IRealEstateServices-i, kasutatakse RealEstateServices-i.
            services.AddScoped<IRealEstateServices, RealEstateServices>();

            // Registreerib andmebaasi konteksti
            services.AddDbContext<ShopTARpe25Context>(x =>
            {
                // Seadistab kasutama mälusisest andmebaasi nimega "TEST", et testimisel kasutada
                //ilma päris andmebaasita
                x.UseInMemoryDatabase("TEST");

                // Eirab hoiatust, sest mälus töötav andmebaas ei toeta transaktsioone.
                x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            // Kutsub välja macrode klassi
            RegisterMacros(services);
        }

        //vabastab ressursid peale kasutamist
        public void Dispose()
        {

        }

        // Abimeetod. Leiab teenuste hulgast soovitud teenuse.
        protected T Svc<T>()
        {
            //otsib teenusepakkuja tüüpi
            return serviceProvider.GetService<T>();
        }

        // Abimeetod makrode leidmiseks ja registreerimiseks
        private void RegisterMacros(IServiceCollection services)
        {
            //saaks reflektsiooniga otsingus kasutada
            var macroBaseType = typeof(IMacros);

            //loeb kokku kõik tüübid sellest assembly'st, kus on määratud
            var macros = macroBaseType.Assembly.GetTypes()
                //valib tüübid, mis rakendavad IMacros liidest, aga ei ole ise liidesed
                //ega abtraksed klassid
                .Where(t => macroBaseType.IsAssignableFrom(t)
                && !t.IsInterface && !t.IsAbstract);

            //käib leitud makrode classid ükshaaval läbi
            foreach (var macro in macros)
            {
                // Kasutab ühte eksemplari kogu teenusepakkuja töö jooksul.
                services.AddSingleton(macro);
            }
        }
    }
}