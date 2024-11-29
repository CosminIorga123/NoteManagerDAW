import { Component, OnInit } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { NoteService } from '../services/note.service';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Category } from '../category';
import { Note } from '../note';

@Component({
  selector: 'app-add-note',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    CommonModule,
    FormsModule,
    MatButtonModule,
    RouterModule,
  ],
  templateUrl: './add-note.component.html',
  styleUrl: './add-note.component.scss',
})
export class AddNoteComponent implements OnInit {

  /**
   * Creates an instance of AddNoteComponent.
   * @param {NoteService} noteService - The service to manage notes.
   * @param {Router} router - The router service for navigation.
   * @param {ActivatedRoute} route - The current activated route.
   */
  constructor(private noteService: NoteService, private router : Router, private route : ActivatedRoute) {}

  /**
   * The ID of the note being edited, or null if adding a new note.
   * @type {string | null}
   */
  noteId: string | null = null;

  /**
   * The title of the note.
   * @type {string}
   */
  noteTitle: string = '';

  /**
   * The description of the note.
   * @type {string}
   */
  noteDescription: string = '';

  /**
   * Array of categories.
   * @type {Category[]}
   */
  categories: Category[] = [];

  /**
   * The ID of the category the note belongs to.
   * @type {string}
   */
  idCategoryNote: string = '';

  /**
   * Error message to display in case of an error.
   * @type {string}
   */
  errorMessage: string = '';

  /**
   * Initializes the component, fetching categories and checking for query parameters.
   */
  ngOnInit(): void {
    this.noteService.getCategories().subscribe((categories: Category[]) => {
      this.categories = categories;
    });

    this.route.queryParams.subscribe(params => {
      if (params['id']) {
        this.noteId = params['id'];
        this.noteTitle = params['title'];
        this.noteDescription = params['description'];
        this.idCategoryNote = params['categoryId'];
      }
    });
    console.log(this.noteTitle);
  }

  /**
   * Adds a new note using the note service.
   */
  addNote(): void {
    this.noteService
      .addNote(this.noteTitle, this.noteDescription, this.idCategoryNote)
      .subscribe({
        next: () => {
          this.errorMessage = 'Note added successfully';
          this.router.navigate(['/']);
        },
        error: (error) =>{
          this.errorMessage = error.message;
        }
      });
  }

  /**
   * Updates an existing note using the note service.
   */
  updateNote(): void {
    if(this.noteId){
      const updatedNote: Note = {
        id: this.noteId,
        title: this.noteTitle,
        description: this.noteDescription,
        categoryId: this.idCategoryNote
      };
      this.noteService.updateNote(updatedNote).subscribe({
        next: () => {
          this.errorMessage = 'Note updated successfully';
          this.router.navigate(['/']);
        },
        error: (error) => {
          this.errorMessage = error.message;
        }
      });
    }
  }

  /**
   * Handles the change event for the category selection.
   * @param {MatSelectChange} event - The selection change event.
   */
  onCategoryChange(event: MatSelectChange): void {
    this.idCategoryNote = event.value;
  }
}
