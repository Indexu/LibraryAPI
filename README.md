# Library API

LibraryAPI is a RESTful API for loaning out books to users, the users reviewing the books and getting recommendations on what to read next.

## Features

* Perform CRUD operations on books and users
    * User email and book ISBN are unique
* Pagination
    * Specify the query parameters `pageNumber` and `pageSize`
* PATCH requests for partial updating
    * Every PUT request can also be a PATCH request
* Persistant storage via SQLite
* Users can
    * Get a book loaned
    * Return a book
    * Review a book (give rating)
    * View reviews and loans
    * Get book recommendations
        * Based on highest rated book in the database the user has not already gotten loaned or reviewed
    * Update a loan and review
    * Get a loan report by book or user
        * Specify the query parameters `loanDate` and/or `duration` in days for either the `/users` or `/books` route
* Heavily tested
    * Over 270 unit tests!
    * Over 250 integration requests with over 900 assertions!
* Commented
    * Every file commented with an XML summary
    * All interfaces define exactly what they do, what their parameters mean and what their return value is
    * All models commented with what their members represent
* This swaggin README

## Getting Started

Run the `dotnet run` in the LibraryAPI/API/src/ directory to build and run.

The server is on port 5000 by default (http://localhost:5000)

```
$ .../LibraryAPI/src/API/> dotnet run
```

You may need to go inside each sub-directory of the LibraryAPI/src/ directory and run
`dotnet restore` if dependencies are missing and not automatically downloaded.

```
$ .../LibraryAPI/src/API/> dotnet restore
$ .../LibraryAPI/src/Exceptions/> dotnet restore
$ .../LibraryAPI/src/Interfaces/> dotnet restore
...
```

### Resetting the database

You can reset the database by overwriting the `LibraryAPI.db` file with the `LibraryAPI.db.backup.db` file in the directory LibraryAPI/src/Repositories/

### Prerequisites

[DotNet Core](https://www.microsoft.com/net/core)

## Implementation details

The API is built with testability in mind. All of the classes depend on interfaces instead of concrete implementations (except for DbContext in the repositories, this is due to Entity Framework Core).
The interfaces are injected into the constructors of the classes using the built-in DotNet Core dependency injection (see the LibaryAPI/src/API/Startup.cs file for specifics).

Unfortunately there is no way of generating a code coverage report for DotNet Core on Mac, but due to my extensive testing I am certain I have 100% code coverage (hopefully).

## Testing

### Unit testing

To run the unit tests, execute the command `dotnet test` in the Tests directory under LibraryAPI/src/

```
$ .../LibraryAPI/src/Tests/> dotnet test
```

### Integration testing

To run the integration tests:
    * Import the LibraryAPI.postman_collection.json file into Postman
    * Start the API server
    * Run the tests using the Collection Runner in Postman

#### Prerequisites

[Postman](https://www.getpostman.com/)

## Final thoughts

I worked alone in this assignment due to having already made the API in the first assignment in order to get a headstart in the this assignment. I had to do minimal refactoring, add the new required features and extensively test my solution.

Testing took way more time than I thought and had little time to do any extravagant bonuses, but I think considering I was at this alone I did a pretty good job.

## Built With

* [Visual Studio Code](https://code.visualstudio.com/)
* [DotNet Core 2.0](https://www.microsoft.com/net/core)
* [Entity Framework Core](https://github.com/aspnet/EntityFrameworkCore)
* [SQLite](https://www.sqlite.org/)
* [Postman](https://www.getpostman.com/)
* [AutoMapper](http://automapper.org/)
* [Moq](https://github.com/moq/moq4)

## Author

* [Hilmar Tryggvason](https://github.com/Indexu/)

## License

Licensed under the [MIT License](https://opensource.org/licenses/MIT)
