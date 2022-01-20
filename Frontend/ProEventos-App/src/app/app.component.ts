import { Component } from '@angular/core';
import { User } from './models/identity/User';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ProEventos-App';

  constructor(public userService: UserService) { }

   // tslint:disable-next-line: use-lifecycle-interface
   ngOnInit(): void {
    this.setCurrentUser();
   }

   setCurrentUser(): void {
    let user: User;

    if (localStorage.getItem('user')) {
      user = JSON.parse(localStorage.getItem('user') ?? '{}');
    } else {
      user = null;
    }

    if (user) {
      this.userService.setCurrentUser(user);
    }
   }
}
