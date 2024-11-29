import { Component } from '@angular/core';
import { FilterComponent } from "../filter/filter.component";
import { ToolsComponent } from "../tools/tools.component";
import { NoteComponent } from "../note/note.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FilterComponent, ToolsComponent, NoteComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

  /**
   * The ID of the selected category used for filtering notes.
   * @type {string}
   */
  categoryId: string = '';

  /**
   * Receives the selected category ID from the FilterComponent.
   * @param {string} categId - The ID of the selected category.
   */
  receiveCategory(categId: string): void {
    this.categoryId = categId;
  }
}
