import { Component, OnChanges, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { Note } from '../note';
import { CommonModule } from '@angular/common';
import { NoteService } from '../services/note.service';
import { Input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { Category } from '../category';
import { Router } from '@angular/router';

@Component({
  selector: 'app-note',
  standalone: true,
  imports: [MatCardModule, CommonModule, MatButtonModule],
  templateUrl: './note.component.html',
  styleUrl: './note.component.scss',
})
export class NoteComponent implements OnInit, OnChanges {
  /**
   * Creates an instance of NoteComponent.
   * @param {NoteService} noteService - The service to manage notes.
   * @param {Router} router - The router service for navigation.
   */
  constructor(private noteService: NoteService, private router: Router) {}

  /**
   * Array of notes.
   * @type {Note[]}
   */
  notes: Note[] = [];

  /**
   * Array of categories.
   * @type {Category[]}
   */
  categories: Category[] = [
    { name: 'To Do', id: '1' },
    { name: 'Done', id: '2' },
    { name: 'Doing', id: '3' },
  ];

  /**
   * Name of the category.
   * @type {string}
   */
  categoryName: string = '';

  /**
   * ID of the selected category.
   * @type {string}
   */
  @Input() selectedCategoryId: string = '';

  /**
   * Initializes the component and fetches all notes.
   */
  ngOnInit() {
    this.fetchAllNotes();
  }

  /**
   * Responds to changes in the input properties.
   */
  ngOnChanges(): void {
    if (this.selectedCategoryId) {
      this.noteService
        .getFilteredNotes(this.selectedCategoryId)
        .subscribe((notes: Note[]) => {
          this.notes = notes;
        });
    } else {
      this.fetchAllNotes();
    }
  }

  /**
   * Fetches all notes from the service.
   */
  fetchAllNotes(): void {
    this.noteService
      .getNotes()
      .subscribe((notes: Note[]) => (this.notes = notes));
  }

  /**
   * Gets the name of the category based on its ID.
   * @param {string} categoryId - The ID of the category.
   * @returns {string} - The name of the category.
   */
  getCategoryName(categoryId: string): string {
    const category = this.categories.find(
      (category) => category.id === categoryId
    );
    return category ? category.name : 'Unknown';
  }

  /**
   * Deletes a note by its ID.
   * @param {string} noteId - The ID of the note to delete.
   */
  deleteNote(noteId: string): void {
    this.noteService.deleteNote(noteId).subscribe({
      next: () =>
        this.noteService
          .getFilteredNotes(this.selectedCategoryId)
          .subscribe((notes: Note[]) => {
            this.notes = notes;
          }),
      error: (error) => console.error('Error deleting note', error),
    });
  }

  /**
   * Navigates to the edit note page with the note details.
   * @param {Note} note - The note to edit.
   */
  editNote(note: Note): void {
    this.router.navigate(['/add-note'], {
      queryParams: {
        id: note.id,
        title: note.title,
        description: note.description,
        categoryId: note.categoryId,
      },
    });
  }
}
