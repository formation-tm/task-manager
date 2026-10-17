namespace TaskManager.Api.Services
{
    public class MySingletonService
    {
        /**
         * j'ai crée cette classe pour tester le piège de "captive dependency" :
         * le Singleton gardait une référence vers le Scoped, ce dernier ne serait jamais libéré
         */
        /**
         * private readonly MyScopedService _scoped;

             public MySingletonService(MyScopedService scoped)
             {
                 _scoped = scoped;
             }

             public Guid GetScopedId() => _scoped.Id;
        */
        /**
         * solution recomandée par microsoft l'utilisation d'une factory : créer un scope manuellement quand un Singleton a vraiment besoin 
         * d’un service Scoped (par exemple un service d’arrière-plan qui utilise le DbContext
         */
        private readonly IServiceProvider _provider;

        public MySingletonService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Guid GetScopedId()
        {
            var scoped = _provider.GetRequiredService<MyScopedService>();
            return scoped.Id;
        }
    }
}
