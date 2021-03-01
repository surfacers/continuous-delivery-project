export interface RaceStatisticEntryDto {
    currentPosition: number;
    deltaPosition: number | null;
    skierId: number;
    time: string;
    deltaTimeLeadership: string;
}
