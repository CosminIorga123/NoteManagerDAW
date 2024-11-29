import { Injectable } from '@angular/core';
import { Note } from '../note';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { catchError, map, Observable, throwError } from 'rxjs';
import { Category } from '../category';

@Injectable({
  providedIn: 'root',
})
export class NoteService {
  /**
   * Holds the array of notes.
   * @type {Note[]}
   */
  notes: Note[] = [];

  /**
   * Holds the array of filtered notes.
   * @type {Note[]}
   */
  filteredNotes: Note[] = [];

  /**
   * Holds the array of categories.
   * @type {string[]}
   */
  categories: string[] = [];

  /**
   * Base URL for the API endpoints.
   * @type {string}
   */
  readonly baseUrl = 'https://localhost:5001';

  /**
   * HTTP options for requests.
   * @type {Object}
   */
  readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  /**
   * Creates an instance of NoteService.
   * @param {HttpClient} http - The HTTP client for making requests.
   */
  constructor(private http: HttpClient) {}

  /**
   * Handles HTTP errors.
   * @param {HttpErrorResponse} error - The HTTP error response.
   * @returns {Observable<never>} - An observable that throws an error.
   */
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'Client error. Try again';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      switch (error.status) {
        case 400:
          errorMessage = "Note incomplete";
          break;
        case 401:
          errorMessage = 'Invalid category';
          break;
        case 409:
          errorMessage = 'Note already exists';
          break;
      }
    }
    return throwError(() => new Error(errorMessage));
  }

  /**
   * Logs a message indicating the service was called.
   */
  serviceCall() {
    console.log('Note service was called');
  }
  
  /**
   * Fetches all notes.
   * @returns {Observable<any>} - An observable containing the notes.
   */
  getNotes(): Observable<any> {
    return this.http
      .get(`${this.baseUrl}/notes`, this.httpOptions)
      .pipe(catchError(this.handleError));
  }
  
  /**
   * Deletes a note by ID.
   * @param {string} noteId - The ID of the note to delete.
   * @returns {Observable<void>} - An observable indicating the operation result.
   */
  deleteNote(noteId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/notes/${noteId}`, this.httpOptions)
      .pipe(catchError(this.handleError));
  }

  /**
   * Fetches notes filtered by category ID.
   * @param {string} categoryId - The ID of the category to filter notes by.
   * @returns {Observable<any>} - An observable containing the filtered notes.
   */
  getFilteredNotes(categoryId: string): Observable<any> {
    if (!categoryId || categoryId === '') {
      return this.getNotes();
    }
    return this.http
      .get<Note[]>(`${this.baseUrl}/notes`, this.httpOptions)
      .pipe(
        map((notes) => notes.filter((note) => note.categoryId === categoryId)),
        catchError(this.handleError)
      );
  }

  /**
   * Fetches all categories.
   * @returns {Observable<Category[]>} - An observable containing the categories.
   */

  getCategories() : Observable<Category[]>{
    return this.http.get<Category[]>(`${this.baseUrl}/Notes/GetCategories`);
  }

  /**
   * Gets the category name by ID.
   * @param {string} categoryId - The ID of the category.
   * @returns {string} - The name of the category.
   */
  getCategoryNameById(categoryId: string): string {
    const categoryMapping: { [key: string]: string } = {
      '1': 'To Do',
      '2': 'Done',
      '3': 'Doing'
    };
    return categoryMapping[categoryId] || 'Unknown';
  }

  /**
   * Adds a new note.
   * @param {string} noteTitle - The title of the note.
   * @param {string} noteDescription - The description of the note.
   * @param {string} noteCategoryId - The category ID of the note.
   * @returns {Observable<any>} - An observable indicating the operation result.
   */
  addNote(noteTitle: string, noteDescription: string, noteCategoryId: string): Observable<any> {
    const note = {
      description: noteDescription,
      title: noteTitle,
      categoryId: noteCategoryId,
    };
    console.log(note);

    return this.http
      .post(`${this.baseUrl}/notes`, note, this.httpOptions)
      .pipe(catchError(this.handleError));
  }

  /**
   * Updates an existing note.
   * @param {Note} note - The note to update.
   * @returns {Observable<void>} - An observable indicating the operation result.
   */
  updateNote(note: Note): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/notes/${note.id}`, note, this.httpOptions)
      .pipe(catchError(this.handleError));
  }
}
