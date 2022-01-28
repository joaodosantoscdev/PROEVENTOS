import { BrowserModule } from '@angular/platform-browser';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';

import { CollapseModule } from 'ngx-bootstrap/collapse';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ModalModule } from 'ngx-bootstrap/modal';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgxCurrencyModule } from 'ngx-currency';
import { TabsModule } from 'ngx-bootstrap/tabs';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { EventsComponent } from './components/events/events.component';
import { SpeakersComponent } from './components/speakers/speakers.component';
import { NavComponent } from './shared/nav/nav.component';
import { TitleComponent } from './shared/title/title.component';
import { ContactComponent } from './components/contacts/contact.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ProfileComponent } from './components/user/profile/profile.component';
import { EventListComponent } from './components/events/event-list/event-list.component';
import { EventDetailsComponent } from './components/events/event-details/event-details.component';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegistrationComponent } from './components/user/registration/registration.component';
import { ProfileDetailsComponent } from './components/user/profile/profile-details/profile-details.component';
import { SpeakersListComponent } from './components/speakers/speakers-list/speakers-list.component';
import { SpeakersDetailsComponent } from './components/speakers/speakers-details/speakers-details.component';

import { EventService } from './services/event.service';
import { PartService } from './services/part.service';
import { UserService } from './services/user.service';

import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { DateFormatPipe } from './helpers/DateFormat.pipe';
import { SocialMediasComponent } from './components/social-medias/social-medias.component';

defineLocale('pt-br', ptBrLocale);

@NgModule({
  declarations: [
    AppComponent,
    EventsComponent,
    EventListComponent,
    EventDetailsComponent,
    SpeakersComponent,
    SpeakersListComponent,
    SpeakersDetailsComponent,
    SocialMediasComponent,
    NavComponent,
    TitleComponent,
    ContactComponent,
    DashboardComponent,
    ProfileComponent,
    ProfileDetailsComponent,
    DateFormatPipe,
    UserComponent,
    LoginComponent,
    RegistrationComponent,
    HomeComponent
   ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    CollapseModule.forRoot(),
    TooltipModule.forRoot(),
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TabsModule.forRoot(),
    ModalModule.forRoot(),
    ToastrModule.forRoot({
        timeOut: 2800,
        positionClass: 'toast-bottom-right',
        preventDuplicates: true,
        progressBar: true
      }),
    NgxSpinnerModule,
    FormsModule,
    NgxCurrencyModule,
    PaginationModule
  ],
  providers: [
    EventService,
    PartService,
    UserService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]

})
export class AppModule { }
