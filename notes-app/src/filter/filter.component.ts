import { Component, Output, EventEmitter } from '@angular/core';
import { Category } from '../category';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-filter',
  standalone: true,
  imports: [MatButtonModule, CommonModule],
  templateUrl: './filter.component.html',
  styleUrl: './filter.component.scss'
})
export class FilterComponent {

  /**
   * Event emitter for the selected filter category ID.
   * @type {EventEmitter<string>}
   */
  @Output() emitSelectedFilter = new EventEmitter<string>();

  /**
   * Array of categories to filter notes.
   * @type {Category[]}
   */
  categories: Category[] = [
    { name: 'To Do', id: '1' },
    { name: 'Done', id: '2' },
    { name: 'Doing', id: '3' }
  ];

  /**
   * Emits the selected filter category ID.
   * @param {string} categoryId - The ID of the selected category.
   */
  selectFilter(categoryId: string): void {
    this.emitSelectedFilter.emit(categoryId);
  }

  /**
   * Resets the filter by emitting an empty string.
   */
  resetFilter(): void {
    this.emitSelectedFilter.emit('');
  }
}
