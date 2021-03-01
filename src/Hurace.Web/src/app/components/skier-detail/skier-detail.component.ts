import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngxs/store';
import { ApiResource } from 'src/app/models';
import { SkierDto } from 'src/app/dtos';
import { Observable } from 'rxjs';
import { GetSkierById } from 'src/app/actions';
import { AuthService } from 'src/app/services/auth.service';

@Component({
    selector: 'app-skier-detail',
    templateUrl: './skier-detail.component.html',
    styleUrls: ['./skier-detail.component.scss']
})
export class SkierDetailComponent implements OnInit {
    public skier: ApiResource<SkierDto>;
    public countryCodes$: Observable<string[]>;

    constructor(
        private route: ActivatedRoute,
        public auth: AuthService,
        private store: Store
    ) {
        this.store.select(state => state.skier.selected)
            .subscribe(skier => this.skier = skier);
    }

    ngOnInit() {
        this.route.params.subscribe(async params => {
            const id = +params['id'];
            this.store.dispatch(new GetSkierById(id));
        });
    }
}
