create table Genres
(
    Id          int identity
        primary key,
    Name        nvarchar(255) not null,
    Description nvarchar(max),
    UsageCount  int default 0
)
go

create table Movies
(
    Id               int identity
        primary key,
    TitleRu          nvarchar(255) not null,
    TitleEng         nvarchar(255) not null,
    ReleaseDateWorld date          not null,
    ReleaseDateRu    date          not null
)
go

create table MovieDetails
(
    Id          int identity
        primary key,
    MovieId     int not null
        references Movies,
    RatingIMDb  decimal(3, 1),
    Duration    int,
    Type        nvarchar(255),
    Description nvarchar(max),
    ImageUrl    nvarchar(500)
        constraint DF_MovieDetails_ImageUrl default './assets/img/not-found.jpg',
    Rating      decimal(3, 1)
        constraint DF_MovieDetails_Rating default 0,
    Website     nvarchar(255) default 'pornhub.com'
)
go

create table MovieGenres
(
    Id        int identity
        primary key,
    MovieId   int not null
        constraint FK_MovieGenres_Movies
            references Movies
            on delete cascade,
    GenreId   int not null
        constraint FK_MovieGenres_Genres
            references Genres
            on delete cascade,
    AddedDate datetime default getdate()
)
go

create table Users
(
    Id       int identity
        primary key,
    Username nvarchar(255) not null,
    Login    nvarchar(255) not null
        unique,
    Password nvarchar(255) not null
)
go


