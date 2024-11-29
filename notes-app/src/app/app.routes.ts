import { Routes } from '@angular/router';
import { HomeComponent } from '../home/home.component';
import { RouterModule } from '@angular/router';
import { AddNoteComponent } from '../add-note/add-note.component';

export const routes: Routes = [
    {path:"add-note", component:AddNoteComponent, pathMatch:'full'},
    {path: "", component: HomeComponent, pathMatch:'full'},
    {path:"**", redirectTo: ""}
];

RouterModule.forRoot(routes);
