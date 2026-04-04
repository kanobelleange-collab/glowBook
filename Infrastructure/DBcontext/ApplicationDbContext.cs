using Microsoft.Extensions.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using Application.Common.Interfaces;
using Dapper; // ✅ nécessaire pour ExecuteAsync

namespace Infrastructure.DBcontext
{
    public class ApplicationDbContext : IApplicationDbContext
    {
        private readonly string _connectionString;

        public ApplicationDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        // ✅ Une seule méthode CreateConnection — avec Open()
        public IDbConnection CreateConnection()
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public async Task InitializeAsync()
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(@"
               CREATE TABLE IF NOT EXISTS Etablissements (
    IntId            INT           NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Id               CHAR(36)      NOT NULL UNIQUE,
    Nom              VARCHAR(150)  NOT NULL,
    Adresse          VARCHAR(255)  NOT NULL,
    Quartier         VARCHAR(100)  NOT NULL DEFAULT '',
    Ville            VARCHAR(100)  NOT NULL,
    Telephone        VARCHAR(20)   NOT NULL,
    Email            VARCHAR(150)  NOT NULL,
    Description      TEXT          NULL,
    Note             DOUBLE        NOT NULL DEFAULT 0,
    NoteMoyenne      DOUBLE        NOT NULL DEFAULT 0,
    EstActif         TINYINT(1)    NOT NULL DEFAULT 1,
    IsDeleted        TINYINT(1)    NOT NULL DEFAULT 0,
    DateCreation     DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DateModification DATETIME      DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    Latitude         DOUBLE        NOT NULL DEFAULT 0,
    Longitude        DOUBLE        NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS Photos (
    IntId           INT          NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Id              CHAR(36)     NOT NULL UNIQUE,
    EtablissementId CHAR(36)     NOT NULL,
    UrlPhoto        VARCHAR(500) NOT NULL,
    CONSTRAINT FK_Photos_Etablissement
        FOREIGN KEY (EtablissementId) REFERENCES Etablissements(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS HorairesOuverture (
    IntId           INT        NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Id              CHAR(36)   NOT NULL UNIQUE,
    EtablissementId CHAR(36)   NOT NULL,
    Jour            TINYINT    NOT NULL,
    HeureOuverture  TIME       NOT NULL,
    HeureFermeture  TIME       NOT NULL,
    EstFerme        TINYINT(1) NOT NULL DEFAULT 0,
    CONSTRAINT FK_Horaires_Etablissement
        FOREIGN KEY (EtablissementId) REFERENCES Etablissements(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS EtablissementServices (
    IntId                     INT          NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Id                        CHAR(36)     NOT NULL UNIQUE,
    EtablissementId           CHAR(36)     NOT NULL,
    TypeServiceNom            VARCHAR(50)  NOT NULL,
    SpecialitesTresse         JSON         NULL,
    TypesCheveux              JSON         NULL,
    AccepteHommes             TINYINT(1)   NULL DEFAULT 0,
    AccepteEnfants            TINYINT(1)   NULL DEFAULT 0,
    TypesMassage              JSON         NULL,
    Ambiance                  VARCHAR(50)  NULL,
    DisponibleADomicile       TINYINT(1)   NULL DEFAULT 0,
    DureeMinimaleMinutes      INT          NULL DEFAULT 30,
    ProposeSoinsVisage        TINYINT(1)   NULL DEFAULT 0,
    ProposeEpilation          TINYINT(1)   NULL DEFAULT 0,
    ProposeOnglerie           TINYINT(1)   NULL DEFAULT 0,
    ProposeMaquillage         TINYINT(1)   NULL DEFAULT 0,
    ProposeProtheseOngles     TINYINT(1)   NULL DEFAULT 0,
    ProposeExtensionCils      TINYINT(1)   NULL DEFAULT 0,
    ProposeProtheseCapillaire TINYINT(1)   NULL DEFAULT 0,
    ServicesSpaNom            VARCHAR(100) NULL,
    CONSTRAINT FK_Services_Etablissement
        FOREIGN KEY (EtablissementId) REFERENCES Etablissements(Id) ON DELETE CASCADE,
    UNIQUE KEY UQ_Service_Type (EtablissementId, TypeServiceNom)
);

CREATE TABLE IF NOT EXISTS Prestations (
    IntId        INT           NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Id           CHAR(36)      NOT NULL UNIQUE,
    ServiceId    CHAR(36)      NOT NULL,
    Nom          VARCHAR(150)  NOT NULL,
    Description  TEXT          NULL,
    Prix         DECIMAL(10,2) NOT NULL DEFAULT 0,
    DureeMinutes INT           NOT NULL DEFAULT 0,
    CONSTRAINT FK_Prestations_Service
        FOREIGN KEY (ServiceId) REFERENCES EtablissementServices(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Clients (
    IntId           INT          NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Id              CHAR(36)     NOT NULL UNIQUE,
    Nom             VARCHAR(100) NULL,
    Email           VARCHAR(150) NULL,
    Telephone       VARCHAR(20)  NOT NULL,
    Ville           VARCHAR(100) NOT NULL,
    Quartier        VARCHAR(100) NULL,
    DateInscription DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    EstActif        TINYINT(1)   NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Employees (
    IntId           INT          NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Id              CHAR(36)     NOT NULL UNIQUE,
    EtablissementId CHAR(36)     NOT NULL,
    Nom             VARCHAR(100) NOT NULL,
    Prenom          VARCHAR(100)  NOT NULL,
    Email           VARCHAR(150) NULL,
    Telephone       VARCHAR(20)  NULL,
    Specialite      VARCHAR(100) NULL,
    UrlPPhoto       VARCHAR(500) NULL,
    AnneeExperience int(10)      NOT NULL,
    
    EstActif        TINYINT(1)   NOT NULL DEFAULT 1,
    DateCreation DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT FK_Employees_Etablissement
        FOREIGN KEY (EtablissementId) REFERENCES Etablissements(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS RendezVous (
    IntId            INT           NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Id               CHAR(36)      NOT NULL UNIQUE,
    ClientId         CHAR(36)      NOT NULL,
    PraticienId      CHAR(36)      NOT NULL,
    ServiceId        CHAR(36)      NOT NULL,
    EtablissementId  CHAR(36)      NOT NULL,
    DateHeure        DATETIME      NOT NULL,
    Statut           VARCHAR(20)   NOT NULL,
    Prix             DECIMAL(10,2) NOT NULL,
    NotesClient      TEXT          NULL,
    RaisonAnnulation TEXT          NULL,
    DateCreation     DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT FK_RDV_Client        FOREIGN KEY (ClientId)        REFERENCES Clients(Id),
    CONSTRAINT FK_RDV_Employee    FOREIGN KEY (EmployeeId)     REFERENCES Praticiens(Id),
    CONSTRAINT FK_RDV_Etablissement FOREIGN KEY (EtablissementId) REFERENCES Etablissements(Id)
);

CREATE TABLE IF NOT EXISTS Paiements (
    IntId         INT           NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Id            CHAR(36)      NOT NULL UNIQUE,
    RendezVousId  CHAR(36)      NOT NULL,
    Montant       DECIMAL(10,2) NOT NULL,
    Statut        VARCHAR(20)   NOT NULL,
    DatePaiement  DATETIME      NULL,
    TransactionId VARCHAR(100)  NULL,
    CONSTRAINT FK_Paiements_RDV FOREIGN KEY (RendezVousId) REFERENCES RendezVous(Id)
);

CREATE TABLE IF NOT EXISTS Avis (
    IntId           INT      NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Id              CHAR(36) NOT NULL UNIQUE,
    ClientId        CHAR(36) NOT NULL,
    EtablissementId CHAR(36) NOT NULL,
    Note            INT      NOT NULL,
    Commentaire     TEXT     NULL,
    DateAvis        DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT FK_Avis_Client        FOREIGN KEY (ClientId)        REFERENCES Clients(Id),
    CONSTRAINT FK_Avis_Etablissement FOREIGN KEY (EtablissementId) REFERENCES Etablissements(Id)
);

CREATE INDEX IF NOT EXISTS IX_Etablissements_Ville    ON Etablissements(Ville);
CREATE INDEX IF NOT EXISTS IX_Etablissements_EstActif ON Etablissements(EstActif);
CREATE INDEX IF NOT EXISTS IX_Etablissements_Geo      ON Etablissements(Latitude, Longitude);
CREATE INDEX IF NOT EXISTS IX_Services_Type           ON EtablissementServices(TypeServiceNom);
CREATE INDEX IF NOT EXISTS IX_RDV_Client              ON RendezVous(ClientId);
CREATE INDEX IF NOT EXISTS IX_Avis_Etablissement      ON Avis(EtablissementId);
            ");
        }
    }
}