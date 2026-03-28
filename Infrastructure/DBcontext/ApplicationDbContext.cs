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
                CREATE TABLE IF NOT EXISTS Clients (
                    Id CHAR(36) PRIMARY KEY,
                    Nom VARCHAR(100),
                    Email VARCHAR(150),
                    Telephone VARCHAR(20) NOT NULL,
                    Ville VARCHAR(100) NOT NULL
                );

                CREATE TABLE IF NOT EXISTS RendezVous (
                    Id CHAR(36) PRIMARY KEY,
                    ClientId CHAR(36) NOT NULL,
                    PraticienId CHAR(36) NOT NULL,
                    ServiceId CHAR(36) NOT NULL,
                    EtablissementId CHAR(36) NOT NULL,
                    DateHeure DATETIME NOT NULL,
                    Statut VARCHAR(20) NOT NULL,
                    Prix DECIMAL(10,2) NOT NULL,
                    NotesClient TEXT,
                    RaisonAnnulation TEXT,
                    DateCreation DATETIME NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Paiements (
                    Id CHAR(36) PRIMARY KEY,
                    RendezVousId CHAR(36) NOT NULL,
                    Montant DECIMAL(10,2) NOT NULL,
                    Statut VARCHAR(20) NOT NULL,
                    DatePaiement DATETIME
                );
            ");
        }
    }
}