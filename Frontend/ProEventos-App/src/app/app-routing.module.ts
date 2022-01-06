import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { EventsComponent } from './components/events/events.component';
import { SpeakersComponent } from './components/speakers/speakers.component';
import { ContactComponent } from './components/contacts/contact.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ProfileComponent } from './components/profile/profile.component';
import { EventDetailsComponent } from './components/events/event-details/event-details.component';
import { EventListComponent } from './components/events/event-list/event-list.component';

const routes: Routes = [
  { path: 'events', redirectTo: 'events/list'},
  {
    path: 'events', component: EventsComponent,
    children: [
      { path: 'details/:id', component: EventDetailsComponent },
      { path: 'details', component: EventDetailsComponent },
      { path: 'list', component: EventListComponent }
    ]
  },
  { path: 'speakers', component: SpeakersComponent },
  { path: 'contacts', component: ContactComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'profile', component: ProfileComponent },
  { path: '**', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
