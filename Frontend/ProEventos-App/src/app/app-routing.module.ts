import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SpeakersComponent } from './components/speakers/speakers.component';
import { ContactComponent } from './components/contacts/contact.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { EventsComponent } from './components/events/events.component';
import { EventDetailsComponent } from './components/events/event-details/event-details.component';
import { EventListComponent } from './components/events/event-list/event-list.component';
import { UserComponent } from './components/user/user.component';
import { ProfileComponent } from './components/user/profile/profile.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegistrationComponent } from './components/user/registration/registration.component';

import { AuthGuard } from './guard/auth.guard';
import { HomeComponent } from './home/home.component';
import { SpeakersListComponent } from './components/speakers/speakers-list/speakers-list.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: '', redirectTo: 'home', pathMatch: 'full'},
  { path: 'user', redirectTo: 'user/profile' },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'user/profile', component: ProfileComponent },
      { path: 'events', redirectTo: 'events/list' },
      {
        path: 'events', component: EventsComponent,
        children: [
          { path: 'details/:id', component: EventDetailsComponent },
          { path: 'details', component: EventDetailsComponent },
          { path: 'list', component: EventListComponent },
        ]
      },
      { path: 'speakers', redirectTo: 'speakers/list' },
      { path: 'speakers', component: SpeakersComponent,
      children: [
          { path: 'list', component: SpeakersListComponent },
        ]
      },
      { path: 'contacts', component: ContactComponent },
      { path: 'dashboard', component: DashboardComponent }
    ]
  },
    {
    path: 'user', component: UserComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'registration', component: RegistrationComponent }
    ]
  },
  { path: '**', redirectTo: '/home', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy', enableTracing: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
