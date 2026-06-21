export interface Track {
  id: number;
  title: string;
  bpm: number;
  duration: string;
  artistId: number;
  artistName?: string;
  playlistId?: number | null;
}
