using Microsoft.Data.SqlClient;
using my_http.Models.Entities;
using my_http.Models.MainPage;
using MyHttp.Repositories;
using MyORMLibrary;

namespace my_http.Repositories;

public class MovieCardRepository
{
    private readonly Repository<Movie> _movieRepository;
    private readonly Repository<MovieDetail> _movieDetailRepository;
    private readonly Repository<MovieGenre> _movieGenreRepository;
    private readonly Repository<Genre> _genreRepository;

    public MovieCardRepository()
    {
        _movieRepository = new Repository<Movie>();
        _movieDetailRepository = new Repository<MovieDetail>();
        _movieGenreRepository = new Repository<MovieGenre>();
        _genreRepository = new Repository<Genre>();
    }

    public List<MovieCardVm> GetMovieCards()
    {
        var movies = _movieRepository.GetAll();

        var movieDetails = _movieDetailRepository.GetAll();

        var movieGenres = _movieGenreRepository.GetAll();

        var genres = _genreRepository.GetAll();

        var movieCards = movies.Select(movie =>
        {
            var movieDetail = movieDetails.FirstOrDefault(md => md.MovieId == movie.Id);

            var genreIds = movieGenres.Where(mg => mg.MovieId == movie.Id).Select(mg => mg.GenreId).ToList();
            var movieGenresNames = genres.Where(g => genreIds.Contains(g.Id)).Select(g => g.Name).ToList();

            return new MovieCardVm
            {
                TitleRu = movie.TitleRu,
                TitleEng = movie.TitleEng,
                ReleaseYear = movie.ReleaseDateWorld.Year.ToString(),
                Genre = movieGenresNames.Count > 0 ? string.Join(", ", movieGenresNames) : "",
                Rating = movieDetail?.Rating.ToString("0.0") ?? "N/A",
                ImageUrl = movieDetail?.ImageUrl ?? "./assets/img/not-found.jpg"
            };
        }).ToList();

        return movieCards;
    }
}