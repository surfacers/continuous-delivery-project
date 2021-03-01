import { Component, OnInit, Input } from '@angular/core';
import { RaceDto, LocationDto } from 'src/app/dtos';
import { formatGender, formatRaceType } from 'src/app/util';

@Component({
  selector: 'app-race-info',
  templateUrl: './race-info.component.html',
  styleUrls: ['./race-info.component.scss']
})
export class RaceInfoComponent {
    @Input() public race: RaceDto;
    @Input() public locations: LocationDto[] = [];

    getLocation(id: number): LocationDto {
        return this.locations.find(l => l.id === id);
    }

    formatGender = formatGender;
    formatRaceType = formatRaceType;
}
