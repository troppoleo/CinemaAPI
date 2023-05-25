----------------------------------------------------------------------------------------------------
DB con accesso tramite win authentication oppure tramire utenza e pwd:
user DB:
cineUser
pwd:
cineUser123!

nel progetto sono contenuti lo zip del backup e il full script completo di dati del DB da usare in alternativa per il restore del DB

----------------------------------------------------------------------------------------------------

Stuttura del progetto secondo il modello:
FrontEnd “API”
BusinessLogic (“BL”)
Data Access Layer (“DAL”) per l’accesso al DB
	Completamente gestito da EF (v7 .net)
Data Transfert Object (“DTO”) per il mappaggio dei dati in base alle necessità specifiche

Isolamento delle DLL:
	API -> Comunica con la sola “BL”
		> referenze nuget:
			>> AutoMapper.Extensions.Microsoft.DependencyInjection (12.0.1) -> inizialmente ho provato ad usarlo in qualche punto c’è qualche implementazione ma ho poi preferito fare il mapping a mano
			>> Microsoft.AspNetCore.SignalR (1.1.0) (ho scoperto essere deprecato, valutare con cosa sostituire) ==> SignalR is now contained in the Microsoft.AspNetCore.App framework. https://learn.microsoft.com/en-us/answers/questions/958531/the-package-microsoft-aspnetcore-signalr-is-deprec
			>> Swashbuckle.AspNetCore (6.2.3)	(per swagger)
			>> Swashbuckle.AspNetCore.Filters (6.2.3)	(per swagger, “forse” l’autenticazione)
	BL -> comunica con DAL e DTO
		> referenze nuget:
			>> AutoMapper (12.0.1) -> inizialmente ho provato ad usarlo in qualche punto c’è qualche implementazione ma ho poi preferito fare il mapping a mano
			>> Microsoft.AspNetCore.Authentication.JwtBearer (6.0.16) -> per l’autenticazione
			>> System.IdentityModel.Tokens.Jwt (6.29.0) -> per l’autenticazione

	DAL -> contiene solo referenze esterne nuget utili a EF: 
		> referenze nuget:
			>> Microsoft.EntityFrameworkCore (7.0.5)
			>> Microsoft.EntityFrameworkCore.Design (7.0.5)
			>> Microsoft.EntityFrameworkCore.SqlServer (7.0.5)
			>> Microsoft.EntityFrameworkCore.Tools (7.0.5)
	DTO -> non ha referenze e non ha dipendenze

