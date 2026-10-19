Définition des trois durées de vie
1. Singleton : une seule instance pour toute la durée de vie de l’application
2. Scoped : une instance par requête HTTP (par scope)
3. Transient : une nouvelle instance à chaque injection
Un cas d’usage concret pour chacun (ex. : cache en mémoire, DbContext, service léger sans état)
Question: Un piège : injecter un service Scoped dans un Singleton (captive dependency). 
Explique pourquoi c’est un problème (le service Scoped reste « capturé » et vit beaucoup trop longtemps.
avec des risques de données périmées ou de concurrence) et comment ASP.NET Core peut le détecter (validation des scopes en développement).
réponse:
« Les trois durées de vie sont Singleton, une instance pour toute l’application, Scoped, une par requête, et Transient, une nouvelle à chaque injection. Le piège classique, 
c’est la captive dependency : un Singleton qui injecte un Scoped. Le Scoped reste capturé et vit beaucoup trop longtemps. Avec un DbContext, 
il serait partagé entre toutes les requêtes, sans thread-safety, avec des données périmées. ASP.NET Core le détecte au démarrage en développement grâce à ValidateScopes. 
Pour corriger, soit on change la durée de vie, soit on injecte IServiceScopeFactory dans le Singleton et on crée un scope pour chaque unité de travail. 
Je l’ai d’ailleurs reproduit dans mon projet. »