using System.Data;
using System.Globalization;
using System.Text;
using HttpServerLibrary.Models;
using Microsoft.Data.SqlClient;
using my_http.Models.FilmPage;

namespace my_http.Repositories;

public class FilmPageDataRepository
{
    private readonly string _connectionString;

    public FilmPageDataRepository()
    {
        var connection = AppConfig.GetInstance().ConnectionString;
        _connectionString = connection;
    }
    
    public FilmPageDataVm GetMovieById(int movieId)
    {
        FilmPageDataVm movie = null;

        string query = @"
            SELECT 
                m.Id,
                m.TitleRu,
                m.TitleEng,
                CONVERT(varchar, m.ReleaseDateWorld, 104) AS ReleaseDateWorld,
                CONVERT(varchar, m.ReleaseDateRu, 104) AS ReleaseDateRu,
                ISNULL(md.RatingIMDb, 0) AS ImdbRating,
                ISNULL(md.Rating, 0) AS Rating,
                ISNULL(md.Duration, 0) AS Duration,
                ISNULL(md.Website, 0) AS Website,
                ISNULL(md.ImageUrl, 0) AS ImageUrl,
                ISNULL(md.Type, '') AS Type,
                STRING_AGG(g.Name, ', ') AS GenreNames,
                ISNULL(md.Description, '') as Description
            FROM MoviesCatalog.dbo.Movies m
            LEFT JOIN MoviesCatalog.dbo.MovieDetails md ON m.Id = md.MovieId
            LEFT JOIN MoviesCatalog.dbo.MovieGenres mg ON m.Id = mg.MovieId
            LEFT JOIN MoviesCatalog.dbo.Genres g ON mg.GenreId = g.Id
            WHERE m.Id = @MovieId
            GROUP BY 
                m.Id, m.TitleRu, m.TitleEng, m.ReleaseDateWorld, m.ReleaseDateRu, 
                md.RatingIMDb, md.Rating, md.Website, md.Duration, md.Type, md.ImageUrl, md.Description
        ";

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MovieId", movieId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        movie = new FilmPageDataVm
                        {
                            TitleRu = reader["TitleRu"].ToString(),
                            TitleEng = reader["TitleEng"].ToString(),
                            ReleaseDateWorld = FormatDate(reader["ReleaseDateWorld"].ToString()),
                            ReleaseDateRu = FormatDate(reader["ReleaseDateRu"].ToString()),
                            ImdbRating = Convert.ToDecimal(reader["ImdbRating"]),
                            Rating = Convert.ToDecimal(reader["Rating"]),
                            Duration = reader["Duration"].ToString() + " мин.",
                            Type = reader["Type"].ToString(),
                            GenreNames = reader["GenreNames"].ToString(),
                            Website = reader["Website"].ToString(),
                            ImageUrl = reader["ImageUrl"].ToString(),
                            Description = reader["Description"].ToString()
                        };
                    }
                }
            }
        }

        return movie;
    }
    
    private string FormatDate(string date)
    {
        if (DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        {
            return parsedDate.ToString("d MMMM yyyy г.", new CultureInfo("ru-RU"));
        }
        return date;
    }
    
}